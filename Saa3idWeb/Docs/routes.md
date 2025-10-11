# Routes

## List of available API routes

| Route | Request Type | Authorization | Description |
| - | - | - | - |
| [api/emergency/](#apiemergency) | GET | None | Obtains an array of emergencies. |
| [api/emergency/{id}](#apiemergencyid) | GET | None | Obtains a single emergency data. |
| [api/emergency/search](#apiemergencysearch) | GET | None | Searches for emergency data from the DB. |
| [api/emergency/create](#apiemergencycreate) | POST | None | Submits an emergency data into the DB. |
| [api/emergency/update/{id}](#apiemergencyupdateid) | PUT | None | Updates an emergency data from the DB. |
| [api/emergency/delete/{id}](#apiemergencydeleteid) | DELETE | User Account | Deletes an emergency data from the DB. |
| [api/hotline/](#apihotline) | GET | None | Obtains an array of hotlines. |
| [api/hotline/{id}](#apihotlineid) | GET | None | Obtains a single hotline data. |
| [api/hotline/search](#apihotlinesearch) | GET | None | Searches for hotline data from the DB. |
| [api/hotline/create](#apihotlinecreate) | POST | User Account | Submits a hotline data into the DB. |
| [api/hotline/update/{id}](#apihotlineupdateid) | PUT | User Account | Updates a hotline data from the DB. |
| [api/hotline/delete/{id}](#apihotlinedeleteid) | DELETE | User Account | Deletes a hotline data from the DB. |
| [api/location/](#apilocation) | GET | None | Obtains an array of locations. |
| [api/location/{id}](#apilocationid) | GET | None | Obtains a single location data. |
| [api/location/search](#apilocationsearch) | POST | None | Searches location data from the DB. |
| [api/location/create](#apilocationcreate) | POST | User Account | Submits a location data into the DB. |
| [api/location/update/{id}](#apilocationupdateid) | PUT | User Account | Updates a location data from the DB. |
| [api/location/delete/{id}](#apilocationdeleteid) | DELETE | User Account | Deletes a location data from the DB. |
| [login/](#login) | POST | None | Authenticates a registered user. |
| [register/](#register) | POST | None | Creates a user account. Does not automatically authenticate the user. |

**Note:** In further versions, some of the routes will require a User account with a **volunteer** membership.

## General rules for APIs
- Each route returns a JSON format.
- APIs that return a **redirect** can be used to navigate into specific pages based on its data. For instance, a **redirect** returning a "home" value shall make its front-end navigate into a homepage. **<u>It should be treated as a signal for page navigation and not an actual route.</u>**
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
- Parameters with "?" are optional.
- All returned DateTime format are YYYY-MM-DDTHH:DD:SSZ with no respect to timezone. It is recommended to manually add additional hours or minutes depending on the timezone. For instance, the timezone for Manila requires adding +08:00 hours into the returned DateTime data.
- Authentication tokens returned by the login route is used in the following format when accessing routes with authorization in CURL format:
```curl
curl -X '[HTTP_REQUEST_TYPE]' \
  '[route_address]' \
  -H 'accept: */*' \
  -H 'Authorization: Bearer [token]
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

### api/emergency/delete/{id}
**Action Name:** API.Emergency.DeleteConfirmed
**Parameters:**
- **Id:** The ID of emergency data.
**Returns:**
- **status:** Returns "OK" if succeeds.
- **redirect:** Returns "home" for homepage.

### api/hotline/
**Returns:**
- **status:** - Returns "OK" if succeeds.
- **hotline** (array) - Each item contains key-value pair of elements

### api/hotline/{id}
**Parameters:**
- **Id** - Hotline ID
**Returns:**
- **hotline:** A single hotline data
- **status:** Returns "OK" if succeeds.

### api/hotline/create
**Request Content:**
- **Id:** (deprecated, to be removed soon) - The ID of the data
- **Type:** Required
- **Number:** Required
- **Neighborhood:** Required
- **City:** Required
- **Province:** Required
**Returns:**
- **hotline:** The newly created hotline data.
- **status:**
    - Returns "Ok" if succeeds.
    - Returns "Error" if fails.
- **redirect:**
    - Returns "hotline/view" if succeeds.
    - Returns "hotline/edit" if fails.

### api/hotline/update/{id}
**Request Content:**
- **Id:** (deprecated, to be removed soon) - The ID of the data
- **Type:** Required
- **Number:** Required
- **Neighborhood:** Required
- **City:** Required
- **Province:** Required
**Returns:**
- **hotline:** The newly created hotline data.
- **status:**
    - Returns "OK" if succeeds.
    - Returns "error" if fails.
- **redirect:** Returns "hotlines" if succeeds.

### api/hotline/delete/{id}
**Parameters:**
- **Id:** - Hotline ID
**Returns:**
- **status:** - Returns "OK" if succeeds.
- **redirect:** - Returns "hotlines".

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

### login/
**Request Content:**
- **username**
- **password**
**Returns:**
- **token:** Authentication token. Used to access routes that require authentication.
- **expiration:** The expiration date of token.
- May also return a JSON consisting of Unauthorized message.

### register/
**Request Content:**
- **UserName:** Required, 3 to 255 characters, string
- **FirstName:** Required, 3 to 255 characters, string
- **MiddleName:** Maximum of 255 characters
- **LastName:** Required, 3 to 255 characters, string
- **ExtName:** Maximum of 255 characters.
- **Gender:** Accepts F and M characters only.
- **Password:** Required, 8 too 255 characters, string [A-z, 0-9] and at least one special character.
- **PhoneNumber:** Proper phone number format.
- **Email:** Required, proper email format
- **HomeAddress:** Required, 3 to 255 characters, string. Must be displayed as "House/Street/Subdivision" on front-end fields.
- **Neighborhood:** Required, 3 to 255 characters, string. Must be displayed like "Brgy." on front-end fields.
- **City:** Required, 3 to 255 characters, string. Must be displayed as "City/Municipality" on front-end fields.

**Returns:**
- **status:**
    - Returns "OK" if succeeds.
    - Returns "Error" otherwise.
- **redirect:**
    - Returns "home" if succeeds.
    - Returns "register" if fails.
- **message**
    - Is only available if an error occurs while checking for user duplicates or during user account creation.
- **result:**
    - Is only available if an error occurs during user account creation.
- **user:**
    - Returns the newly submitted user data.