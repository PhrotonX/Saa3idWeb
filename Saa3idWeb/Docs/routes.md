# Routes

## List of available API routes

| Route | Request Type | Action Name | Description | Parameters | Return Value |
| - | - | - | - | - | - |
| api/emergency/ | GET | API.Emergency.Index | Obtains an array of emergencies. | | emergencies (array) - Each item contains key-value pair of elements |
| api/emergency/{id} | GET | | Obtains a single emergency data. | Id - Emergency ID | emergency, status |
| api/emergency/create | POST | API.Emergency.Create | Submits an emergency data into the DB. | Id,Latitude,Longitude,UserId | emergency, success
| api/emergency/update/{id} | PUT | API.Emergency.Update | Updates an emergency data from the DB. | Id - Emergency ID | status |
| api/emergency/delete/{id} | DELETE | API.Emergency.DeleteConfirmed | Deletes an emergency data from the DB. | Id - Emergency ID | status, redirect |
| api/hotline/ | GET |  | Obtains an array of hotline. | | hotline (array) - Each item contains key-value pair of elements |
| api/hotline/{id} | GET | | Obtains a single hotline data. | Id - Hotline ID | hotline, status |
| api/hotline/create | POST |  | Submits a hotline data into the DB. | Id,Latitude,Longitude,UserId | hotline, status
| api/hotline/update/{id} | PUT | | Updates a hotline data from the DB. | Id - Hotline ID | status, redirect, hotline |
| api/hotline/delete/{id} | DELETE |  | Deletes a hotline data from the DB. | Id - Hotline ID | status, redirect |
