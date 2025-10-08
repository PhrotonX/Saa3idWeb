# Routes

## List of available API routes

| Route | Request Type | Action Name | Description | Parameters | Return Value |
| - | - | - | - | - | - |
| [api/emergency/](#apiemergency) | GET |  | Obtains an array of emergencies. | | |
| [api/emergency/{id}](#apiemergencyid) | GET | | Obtains a single emergency data. | | |
| [api/emergency/search](#apiemergencysearch) | GET | | Searches for emergency data from the DB. | | |
| [api/emergency/create](#apiemergencycreate) | POST | | Submits an emergency data into the DB. |  | 
| [api/emergency/update/{id}](#apiemergencyupdateid) | PUT | | Updates an emergency data from the DB. | | |
| api/emergency/delete/{id} | DELETE | API.Emergency.DeleteConfirmed | Deletes an emergency data from the DB. | Id - Emergency ID | status, redirect |
| api/hotline/ | GET |  | Obtains an array of hotlines. | | hotline (array) - Each item contains key-value pair of elements |
| api/hotline/{id} | GET | | Obtains a single hotline data. | Id - Hotline ID | hotline, status |
| [api/hotline/search]() | GET | | Searches for hotline data from the DB. | | |
| api/hotline/create | POST |  | Submits a hotline data into the DB. | Id,Latitude,Longitude,UserId | hotline, status
| api/hotline/update/{id} | PUT | | Updates a hotline data from the DB. | Id - Hotline ID | status, redirect, hotline |
| api/hotline/delete/{id} | DELETE |  | Deletes a hotline data from the DB. | Id - Hotline ID | status, redirect |
| [api/location/](#apilocation) | GET |  | Obtains an array of locations. | | |
| [api/location/{id}](#apilocationid) | GET | | Obtains a single location data. |  |  |
| [api/location/search](#apilocationsearch) | POST |  | Searches location data from the DB. |  | 
| [api/location/create](#apilocationcreate) | POST |  | Submits a location data into the DB. |  | 
| [api/location/update/{id}](#apilocationupdateid) | PUT | | Updates a location data from the DB. |  |  |
| [api/location/delete/{id}](#apilocationdeleteid) | DELETE |  | Deletes a location data from the DB. |  |  |

## General rules for APIs
- Each route returns a JSON format.
- APIs that return a **redirect** can be used to navigate into specific pages based on its data. For instance, a **redirect** returning a "home" value shall make its front-end navigate into a homepage.
- APIs that return a **status** typically returns "OK" and "Error".
- APIs may return an array of key-value pairs. For instance, a key "hotlines" may have {"title":"Sample Title", "description":"sample Description"} as its value.
- APIs may return HTTP status responses.
    - Error 404 if a specified resource does not exist. For instance, submitting ID 5 on a route wherein the database only consists of item IDs 1-4 throws error 404.
- APIs may require entity models in a JSON format. Typically found in POST, PUT, and PATCH requests. These require setting up JSON keys to correspond into a DB field. For instance, an entity model named Location requiring Title,Description,Latitude,Longitude,LocationType shall have the following JSON data into an HTTP request:
```json
{
    "Title": "Sample Title",
    "Description": "Sample Description",
    "Latitude":"4.56565656",
    "Longitude":"4.76767676",
    "LocationType":"evacuation_center"
}
```
- APIs may require a query string. These query strings starts with a question mark on its URL
    - Spaces on query strings are represented by %20.
    - Multiple queries are split by "&amp;" symbol.
    - Each query string are represented by key=value pair.
    - Example:
```
    http://127.0.0.1:8080/api/emergency/search?title=title%20here&description=this_is_a%20description.
```


## Details
### api/emergency/
**Action Name:** API.Emergency.Index
**Return:**
- **emergencies** (array): Each item contains key-value pair of elements.

### api/emergency/{id}
**Parameters:**
- **Id**
    - **Emergency ID** - Used to retrieve the emergency data based on its ID.
**Returns:**
- **emergency:** A single Emergency data
- **status**
    - Returns **"OK"** if request is valid.

### api/emergency/search
**Query Strings:**
- **userId?**
    - The ID of the associated user account.
- **lat?**
	- Latitude
- **lng?**
	- Longitude
**Returns:**
- **results**
    - Returns an array of emergency data.

### api/emergency/create
**Action Name:** API.Emergency.Create
**Request Content:**
- **Latitude**
- **Longitude**
- **UserId:** The associated user ID or the ID of the user who created the data. It can be 0 if anonymous or not available.
**Returns:**
- **status:**
    - Returns "OK".
- **emergency**
    - Returns the newly created emergency data.
- **success (deprecated)**
    - Returns true.

### api/emergency/update/{id}
**Action Name:** API.Emergency.Update
**Parameters:**
- **id:** The ID of emergency data.
**Request Content:**
- **Latitude**
- **Longitude**
- **UserId:** The associated user ID or the ID of the user who created the data. It can be 0 if anonymous or not available. It is recommended to ignore or avoid changing this data.
**Returns:**
- **status:**
    - Returns "OK" if it succeed, "Error" otherwise.

### api/hotline/search
**Query Strings:**
- **type?**
- **number?**
- **neighborhood?**
- **city?**
- **province?**
**Returns:**
- **results**
    - Returns an array of hotline data.
    
### api/location/
**Returns:**
- **hotline** (array) - Each item contains key-value pair of elements

### api/location/{id}
**Parameters:**
- **Id**
    - **Location ID** - Used to retrieve the location data based on its ID.
**Returns:**
- **status**
    - Returns **"OK"** if model state is valid.
- **redirect**
    - Returns **"location"** if the model state is valid. This shall navigate the front-end into location details page.
- **location** - Returns the location data.

### api/location/search
**Query Strings:**
- **title?**
- **description?**
- **lat?**
	- Latitude
- **lng?**
	- Longitude
**Returns:**
- **results**
    - Returns an array of location data. 

### api/location/create
**Request Content**
- Location model: Title,Description,Latitude,Longitude,LocationType
**Returns:**
- **status**
    - Returns **"OK"** if model state is valid
    - Returns **"Error"** if failed.
- **redirect**
    - Returns **"location"** if the model state is valid. This shall navigate the front-end into location details page.
    - Returns **"location/create"** if failed. This may or may not navigate the front-end back into homepage but with errors displayed.
- **location** - Returns the new location data.

### api/location/update/{id}
**Request Content**
- Location model: Title,Description,Latitude,Longitude,LocationType
**Parameters:**
- **Id** - Location ID
**Returns:**
- **status** - Returns "OK" if model state is valid and "Error" if failed.
- **redirect**
    - Returns "location" if the model state is valid. This shall navigate the front-end into location details page.
    - Returns "location/edit" if failed. This may or may not navigate the front-end back into location edit page but with errors displayed.
- **location** - Returns the updated location data.


### api/location/delete/{id}
**Parameters:** 
- **Id**: Location ID

**Returns:**
- **status**
- **redirect**