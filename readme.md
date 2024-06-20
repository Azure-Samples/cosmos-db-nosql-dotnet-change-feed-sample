# Sample passwordless Azure Cosmos DB for NoSQL change feed solution

TODO

## Create databases and containers in advance

1. Create a single database and two containers. 

1. Set both containers partition key path to `/id`.

## Setup data-plane role-based access control

Use built-in **Cosmos DB Built-in Data Contributor** role: `00000000-0000-0000-0000-000000000002`

```azurecli
# List existing roles
az cosmosdb sql role definition list \
    --account-name <your-account-name> \
    --resource-group <your-resource-group-name>

# Get your principal id
az ad signed-in-user show --query id --output tsv

# Assign your principal access to contribute data
az cosmosdb sql role assignment create \
    --account-name <your-account-name> \
    --resource-group <your-resource-group-name> \
    --scope "/" \
    --principal-id <your-principal-id> \
    --role-definition-id 00000000-0000-0000-0000-000000000002
```
