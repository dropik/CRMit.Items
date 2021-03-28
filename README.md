# CRMit

An example of a CRM system for a generic business. Based on microservice architecture. Implemented with ASP.NET Core.

## Purposes

As the 'CRM' (Customer Relationship Management) prefix in the name CRMit suggests, this system manages the relationship between the customer itself and the business (which in this case is a generic item-selling business). It stores customer data, handles items, purchases and various notifications to customer

# CRMit.Items

Microservice responsible for Items resource in CRMit REST API v1.

## Getting Started

To start the service using database with {Server}, {Port}, {DatabaseName}, {User} and {Password}, run

```cmd
> docker run -e "ASPNETCORE_URLS=http://+" -e "CONNECTIONSTRINGS__ITEMSDB=Server={Server},{Port}; Database={DatabaseName}; User ID={User}; Password={Password};" -p 8000:80 -d dropik/crmit-items:latest
```

To get HTTPS working, mount a directory with certificates like ```${HOME}/.aspnet/https:/https/``` and specify the appropriate environment variables.

## Environment Variables

| Variable name | Description |
| ------------- | ----------- |
| ASPNETCORE_URLS | Host URLs to use. Consider specifying something like "https://+" to use only HTTPS. |
| CONNECTIONSTRINGS__ITEMSDB | Connection string to a Microsoft SQL Server. Please refer the following documentation https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnection.connectionstring?view=dotnet-plat-ext-5.0 to set your connection string properly. |
| ASPNETCORE_Kestrel__Certificates__Default__Path | Path to .pfx or .crt certificate. |
| ASPNETCORE_Kestrel__Certificates__Default__Password | Password if using .pfx certificate. |
| ASPNETCORE_Kestrel__Certificates__Default__KeyPath | Path to .key file if using .crt certificate. |

# REST API Resource: Items

## Item

Represents data of a single item.

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| id | long | Item's unique number.  
_Example:_ `123456` | Yes |
| name | string | Name.  
_Example:_ `"Laptop"` | Yes |
| description | string | Description.  
_Example:_ `"A brand new laptop."` | No |
| price | double | Price.  
_Example:_ `100` | No |

## Operations

### /crmit/v1/Items

#### POST

Create a new item.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| body | body | Item data to be used for new item. | Yes | [ItemInput](#iteminput) |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 201 | Item created and standard Created response returned with new item. | [Item](#item) |
| 400 | Invalid parameters passed. | [ProblemDetails](#problemdetails) |
| 500 | Internal error occured. |  |

#### GET

Get a list containing all items.

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | List with all items. | [ [Item](#item) ] |
| 500 | Internal error occured. |  |

### /crmit/v1/Items/{id}

#### GET

Get an item by id.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path | Item id. | Yes | long |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Found item. | [Item](#item) |
| 400 | Invalid parameters passed. | [ProblemDetails](#problemdetails) |
| 404 | Item not found. | [ProblemDetails](#problemdetails) |
| 500 | Internal error occured. |  |

#### PUT

Update an item given id.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path | Item id. | Yes | long |
| body | body | Item data to update with. | Yes | [ItemInput](#iteminput) |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Item updated. |  |
| 400 | Invalid parameters passed. | [ProblemDetails](#problemdetails) |
| 404 | Item not found. | [ProblemDetails](#problemdetails) |
| 500 | Internal error occured. |  |

#### DELETE

Delete an item given id.

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path | Item id. | Yes | long |

##### Responses

| Code | Description | Schema |
| ---- | ----------- | ------ |
| 200 | Item deleted. |  |
| 400 | Invalid parameters passed. | [ProblemDetails](#problemdetails) |
| 404 | Item not found. | [ProblemDetails](#problemdetails) |
| 500 | Internal error occured. |  |

### Models

#### ItemInput

Item input data.

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| name | string | Name.  
_Example:_ `"Laptop"` | Yes |
| description | string | Description.  
_Example:_ `"A brand new laptop."` | No |
| price | double | Price.  
_Example:_ `100` | No |

#### ProblemDetails

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| type | string |  | No |
| title | string |  | No |
| status | integer |  | No |
| detail | string |  | No |
| instance | string |  | No |

## Changelog

### v1.0.0

- Added basic CRUD operations on Items resource.
