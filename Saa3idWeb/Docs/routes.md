# Routes

## List of available API routes

| Route | Request Type | Action Name | Description | Parameters | Return Value |
| - | - | - | - | - | - |
| [api/emergency/](#apiemergency) | GET |  | Obtains an array of emergencies. | | |
| [api/emergency/{id}]() | GET | | Obtains a single emergency data. | | |
| api/emergency/create | POST | API.Emergency.Create | Submits an emergency data into the DB. | Id,Latitude,Longitude,UserId | emergency, success
| api/emergency/update/{id} | PUT | API.Emergency.Update | Updates an emergency data from the DB. | Id - Emergency ID | status |
| api/emergency/delete/{id} | DELETE | API.Emergency.DeleteConfirmed | Deletes an emergency data from the DB. | Id - Emergency ID | status, redirect |
| api/hotline/ | GET |  | Obtains an array of hotlines. | | hotline (array) - Each item contains key-value pair of elements |
| api/hotline/{id} | GET | | Obtains a single hotline data. | Id - Hotline ID | hotline, status |
| api/hotline/create | POST |  | Submits a hotline data into the DB. | Id,Latitude,Longitude,UserId | hotline, status
| api/hotline/update/{id} | PUT | | Updates a hotline data from the DB. | Id - Hotline ID | status, redirect, hotline |
| api/hotline/delete/{id} | DELETE |  | Deletes a hotline data from the DB. | Id - Hotline ID | status, redirect |
| [api/location/](#apilocation) | GET |  | Obtains an array of locations. | | |
| [api/location/{id}](#apilocationid) | GET | | Obtains a single location data. |  |  |
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

### api/location/create
**Request Parameters**
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
**Request Parameters**
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