cd src/CRMit.Items/bin/Debug/net5.0
dotnet swagger tofile --output swagger.yml --serializeasv2 --yaml CRMit.Items.dll v1
swagger-markdown -i swagger.yml -o swagger.md
