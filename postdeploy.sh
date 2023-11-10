iothubname=$1
adtname=$2
rgname=$3
location=$4
egname=$5
egid=$6
funcappid=$7
storagename=$8
containername=$9

echo "iot hub name: ${iothubname}"
echo "adt name: ${adtname}"
echo "rg name: ${rgname}"
echo "location: ${location}"
echo "egname: ${egname}"
echo "egid: ${egid}"
echo "funcappid: ${funcappid}"
echo "storagename: ${storagename}"
echo "containername: ${containername}"

# echo 'installing azure cli extension'
az config set extension.use_dynamic_install=yes_without_prompt
az extension add --name azure-iot -y

# echo 'retrieve files'
git clone https://github.com/Thiennam209/dt-denso

# echo 'input model'
MachineId=$(az dt model create -n $adtname --models ./dt-denso/models/machine.json --query [].id -o tsv)

# echo 'instantiate ADT Instances'
for i in {1..100}; do
    echo "Create machine Machine$i"
    az dt twin create -n $adtname --dtmi $MachineId --twin-id "Machine$i"
done

for i in {1..4}; do
    az dt twin update -n $adtname --twin-id "Machine$i" --json-patch '[{"op":"add", "path":"/MachineId", "value": "'"Machine$i"'"},{"op":"add", "path":"/Alert", "value": false},{"op":"add", "path":"/Time", "value": "Time"},
    {"op":"add", "path":"/Part", "value": 0},{"op":"add", "path":"/Station", "value": "0"},{"op":"add", "path":"/Serial", "value": 0},{"op":"add", "path":"/AdjJudge", "value": "0"},{"op":"add", "path":"/Pressure", "value": 0},
    {"op":"add", "path":"/IP1", "value": 0},{"op":"add", "path":"/CrimpJudge", "value": "0"},{"op":"add", "path":"/PerformJudge", "value": "0"},{"op":"add", "path":"/I1", "value": 0},{"op":"add", "path":"/I2", "value": 0},
    {"op":"add", "path":"/I3", "value": 0},{"op":"add", "path":"/I4", "value": 0},{"op":"add", "path":"/I1I15", "value": 0},{"op":"add", "path":"/I2I14", "value": 0},{"op":"add", "path":"/I3I13", "value": 0},
    {"op":"add", "path":"/Stick1", "value": 0},{"op":"add", "path":"/Stick3", "value": 0},{"op":"add", "path":"/Flow", "value": 0},{"op":"add", "path":"/Resp1_P5", "value": 0},{"op":"add", "path":"/Resp1_P6", "value": 0},
    {"op":"add", "path":"/Resp2_T2", "value": 0},{"op":"add", "path":"/Resp2_P5", "value": 0},{"op":"add", "path":"/Resp2_P6", "value": 0}]'
done

# az eventgrid topic create -g $rgname --name $egname -l $location
az dt endpoint create eventgrid --dt-name $adtname --eventgrid-resource-group $rgname --eventgrid-topic $egname --endpoint-name "$egname-ep"
az dt route create --dt-name $adtname --endpoint-name "$egname-ep" --route-name "$egname-rt"

# Create Subscriptions
az eventgrid event-subscription create --name "$egname-broadcast-sub" --source-resource-id $egid --endpoint "$funcappid/functions/broadcast" --endpoint-type azurefunction

# Retrieve and Upload models to blob storage
az storage blob upload-batch --account-name $storagename -d $containername -s "./dt-denso/assets"
