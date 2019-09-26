AppLockerLog
======
Dashboard for AppLocker entries.

This was a project i did for Odense Municipalty as an intern.
It was my first time creating an Angular application with a C# backend.
I grew a lot as a developer, as i only had basic experience in php at the time.

The quality of the project isn't that good - but it reflects on how i've improved over time.
It was well recieved, and included in their stack when i left.

I worked on this project from October 2017 to February 2018.

Angular frontend

.NET Web API 2.0

## Index

+ [Usage](#usage)
+ [Config](#config)
+ [Eventlog IDs](#eventlog-ids)
+ [Roles](#roles)
+ [API](#api)
	- [Info](#info)
	- [Angular and DELETE requests](#angular-and-delete-requests)
	- [Endpoints](#endpoints)

## Usage

Use Visual Studio 2017 for building.

```$ git clone git@github.com:martin-juul/odk-applockerlog.git```

Note: VS will build Angular for you.

## Config

### API (backend)

In AppLockerLog.Web you can edit the settings for the API.


 * AppLockerLog.Web
   * Web.Debug.config - For dev environment
   * Web.Release.config - For production environment

SQL Server connection string edit:

```
<add name="DefaultConnection"
	connectionString="server=;database=;user id=;password="
	providerName="System.Data.SqlClient"
	xdt:Transform="Replace" xdt:Locator="Match(name)"/>
```

Site urls

```
	<appSettings>
		<add key="WebApiEndPoint"
		value="protocol://domain.tld/api"
		xdt:Transform="SetAttributes(value)"
		xdt:Locator="Match(key)" />
	<add key="DefaultPage"
		value="protocol://domain.tld"
		xdt:Transform="SetAttributes(value)"
		xdt:Locator="Match(key)" />
	</appSettings>
```

### Angular (Frontend)

 * ApplockerLog.Web\src\

 * environments
   * environment.prod.ts
   * environment.ts

Edit the urls for CORS to work.

```
  serviceBaseUrl: ' ',
  origin: ' '
```


## Eventlog IDs

200 - Information - Created new entry

201 - Information - Entry completed

300 - Information - Deleted entry

666 - Information - New log entry

## Roles

| API Role | AD Role |
|----------|---------|
| incidentReaderGroup | TORG-ODK-W10-USER-APPLOCKERLOG-Read |
| incidentResolverGroup | TORG-ODK-W10-USER-APPLOCKERLOG-Write |


# API

### Info

You need to be authenticated, before you send a request. This happens automatically with NTLM/Windows Authentication.

In Insomnia or Postman, go to the authentication tab, choose Microsoft NTLM, and put in your credentials before sending requests to the api.

### Angular and DELETE requests
HttpClientL will throw up, if you do not set the response type to text. It allows for empty bodies.

```this.http.delete(this.api + endpoint + id, {responseType: 'text'})```

## Endpoints

[Auth](#Auth)

[Log Entries](#log-entries)

[User Approvals](#user-approvals)

[Software Approvals](#software-approvals)

## Auth

```GET: /api/auth/getuser```

Returns a user object.

```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
```

~~~javascript
{
	"userName": "ODKNET\\marjc",
	"incidentReaderGroup": true,
	"incidentResolverGroup": true
}
~~~

## Log entries

#### Single entry

```GET: /api/entry/:id```

Returns a single log entry.

```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"userName": string,
	"computerName": string,
	"ip": string,
	"programDescription": string,
	"rapportDescription": string,
	"note": string|null,
	"timeStamp": string,
	"editedBy": string|null,
	"deniedBy": string|null
}
~~~

#### Paginated results

```GET: /api/entries?pageSize=:int&page=:int```

Paginated Results. Page starts at 0.

Returns the amount of entries you chose in pageSize, and at the specific page number.

```
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
```

~~~javascript
{
	"result": [
		{
			"id": int,
			"userName": string,
			"computerName": string,
			"ip": string,
			"programDescription": string,
			"rapportDescription": string,
			"note": string|null,
			"timeStamp": "2017-12-06T00:00:00",
			"editedBy": string|null,
			"deniedBy": string|null
		},
		{
			...
		}
	],
	"pagination": {
		"totalCount": int,
		"totalPages": int,
		"prevLink": protocol://url/api/entry?page=int&pageSize=int-1,
		"nextLink": protocol://url/api/entry?page=int&pageSize=int+1
	}
}
~~~

#### Paginated search results

```GET: /api/entries?pageSize=:int&page=:int&query=string```

Same as paginated results, but filters the result set by Username | Computername | IP

#### Create entry

```POST: /api/entry```

Post a JSON formatted object like this

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"UserName": "",
	"ComputerName": "",
	"Ip": "",
	"ProgramDescription": "",
	"RapportDescription": ""
}
~~~

The api will return the created object, if successful.

```
HTTP/1.1 201 CREATED
Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"userName": string,
	"computerName": string,
	"ip": string,
	"programDescription": string,
	"rapportDescription": string,
	"note": string|null,
	"timeStamp": string,
	"editedBy": string|null,
	"deniedBy": string|null
}
~~~

#### Update entry

```PUT|PATCH: /api/entry/:id```

Note: This will update all of the fields on the entry, so send the whole object, if you only update a couple of fields.

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"userName": string,
	"computerName": string,
	"ip": string,
	"programDescription": string,
	"rapportDescription": string,
	"note": string|null,
	"timeStamp": string,
	"editedBy": string|null,
	"deniedBy": string|null
}
~~~

Returns the updated object and a ```200 OK``` status code.

#### Update edited by field (Approved)

```PUT|PATCH: /api/entry/:id/resolved```

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"EditedBy": string
}
~~~

Returns status code ```200 OK``` and the object.

#### Update denied by field (Denied)

```PUT|PATCH: /api/entry/:id/denied```

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"deniedBy": string
}
~~~

Returns status code ```200 OK``` and the object.

#### Update notes

```PUT|PATCH: /api/entry/:id/note```

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"note": string
}
~~~

Returns status code ```200 OK``` and the object.

#### Delete entry

```DELETE: /api/entry/:id```

No need to send any body, just send a DELETE request to the specified url with your ID. Needs incidentResolverGroup.

Returns empty body and status code ```200 OK```

## User Approvals

#### Single entry

```GET: /api/approval/:id```

Returns a single user with assigned user groups

```
* Server auth using NTLM with user

HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
```

~~~javascript
{
	"assignedUserGroups": [
		{
			"id": int,
			"approvalID": int,
			"group": string
		},
		{
			...
		}
	],
	"id": int,
	"userName": string,
	"reasoning": string,
	"timeStamp": string,
	"approver": string
}
~~~

#### Paginated results

```GET: /api/entries?pageSize=:int&page=:int```

Paginated Results. Page starts at 0.

Returns the amount of entries you chose in pageSize, and at the specific page number.

```
* Server auth using NTLM with user

HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
```

~~~javascript
{
	"result": [
		{
		"assignedUserGroups": [
			{
				"id": int,
				"approvalID": int,
				"group": string
			},
			{
				...
			}
		],
		"id": int,
		"userName": string,
		"reasoning": string,
		"timeStamp": string,
		"approver": string
	}
	],
	"pagination": {
		"totalCount": int,
		"totalPages": int,
		"prevLink": protocol://url/api/approval?page=int&pageSize=int-1,
		"nextLink": protocol://url/api/approval?page=int&pageSize=int+1
	}
}
~~~

#### Paginated search results

```GET: /api/entries?pageSize=:int&page=:int```

Same as paginated results, but filters the result set by Username.

**NOTE: Currently broken as of 20/12/17**

#### Create entry

```POST: /api/approval```

Post a JSON formatted object like this

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8

```

~~~javascript
{
	"username": string,
	"assignedUserGroups":[
		{
			"group": string
		},
		{
			...
		}
	],
	"reasoning": string
}
~~~

The api will return the created object, if successful.

```
HTTP/1.1 201 CREATED
Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": 149,
	"userName": "marjc",
	"reasoning": "",
	"timeStamp": "2017-12-20T12:43:30.3469049+01:00",
	"approver": "ODKNET\\marjc",
	"assignedUserGroups": [
		{
			"id": 179,
			"approvalID": 149,
			"group": "ODK-W7-SHAREPOINT-MGMT"
		},
		{
			"id": 180,
			"approvalID": 149,
			"group": "ODK-RADIUS-GUEST-WIFI"
		}
	]
}
~~~

#### Update entry

```PUT|PATCH: /api/approval/:id```

Note: This will update all of the fields on the entry, so send the whole object, if you only update a couple of fields.

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"userName": string,
	"reasoning": string,
	"timeStamp": string,
	"approver": string,
	"assignedUserGroups": [
		{
			"id": int,
			"approvalID": int,
			"group" string
		}
	]
}
~~~

Returns the updated object and a ```200``` OK status code.

#### Update note (Reasoning)

```PUT|PATCH: /api/approval/:id/note```

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"reasoning": string
}
~~~

Returns status code ```200 OK``` and the object.

#### Update group (Add group)

```PUT|PATCH: /api/entry/:id/denied```

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"assignedUserGroups": [
		{
			"group": string
		}
	]
}
~~~

Returns status code ```200 OK``` and the object.

#### Delete group

```DELETE: /api/approval/:groupId/group```

**NOTE: you have to get the specific group id, if you do not know it beforehand, query the approval entry and grab the 'id' from 'assignedUserGroups'**

Returns status code ```200 OK``` and empty body.

#### Delete approval

```DELETE: /api/entry/:id```

No need to send any body, just send a DELETE request to the specified url with your ID. 

Returns empty body and status code ```200 OK```

## Software Approvals

#### Paginated results

```GET: /api/software?pageSize=:int&page=:int```

Paginated Results. Page starts at 0.

Returns the amount of entries you chose in pageSize, and at the specific page number.

```
* Server auth using NTLM with user

HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
```

~~~javascript
{
	"result": [
		{
			"id": int,
			"name": string,
			"vendor": string,
			"reasoning": string,
			"timeStamp": string,
			"createdBy": string,
			"state": string
		}
	],
	"pagination": {
		"totalCount": 99,
		"totalPages": 99,
		"prevLink": "protocol://url/api/software?page=int-1&pageSize=int-1",
		"nextLink": "protocol://url/api/software?page=int+1&pageSize=int+1"
	}
}
~~~

```"state":``` will be either approved or denied.

To only get approved software, set the state ```GET``` variable to ```'approved'```

```GET: /api/software?pageSize=:int&page=:int&state=denied|approved```

#### Paginated search results

```GET: /api/software?pageSize=:int&page=:int&state=approved|denied&query=string```

Same as paginated results, but filters the result set by Name | Vendor. With the set state.

#### Create entry

```POST: /api/software```

Post a JSON formatted object like this

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"name": string,
	"vendor": string,
	"reasoning": string,
	"state": "approved" | "denied"
}
~~~

The api will return the created object, if successful.

```
HTTP/1.1 201 CREATED
Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": 97,
	"name": "Bootcamp Utilities",
	"vendor": "Apple Computers Inc.",
	"reasoning": "Drivere til Windows fra Apple",
	"timeStamp": "2017-12-12T12:00:09.1670294+01:00",
	"createdBy": "ODKNET\\marjc",
	"state": "approved"
}
~~~

#### Update entry

```PUT|PATCH: /api/software/:id```

Note: This will update all of the fields on the entry, so send the whole object, if you only update a couple of fields.

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": 97,
	"name": "Bootcamp Utilities",
	"vendor": "Apple Computers Inc.",
	"reasoning": "Drivere til Windows fra Apple, bruges på A1027",
	"timeStamp": "2017-12-12T12:00:09.1670294+01:00",
	"createdBy": "ODKNET\\marjc",
	"state": "denied"
}
~~~

Returns the updated object and a ```200``` OK status code.

#### Update note (Reasoning)

```PUT|PATCH: /api/software/:id/note```

```
* Server auth using NTLM with user

Content-Type application/json; charset=utf-8
```

~~~javascript
{
	"id": int,
	"reasoning": string
}
~~~

Returns status code ```200 OK``` and the object.

#### Delete entry

```DELETE: /api/software/:id```

No need to send any body, just send a DELETE request to the specified url with your ID. 

Returns empty body and status code ```200 OK```