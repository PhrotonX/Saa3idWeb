# Routes

## List of available API routes

| Route | Request Type | Action Name | Description | Parameters | Return Value |
| - | - | - | - | - | - |
| api/emergency/ | GET | API.Emergency.Index | Obtains an array of emergencies. | | emergencies (array) - Each item contains key-value pair of elements |
| api/emergency/create | POST | API.Emergency.Create | Submits an emergency data into the DB. | Id,Latitude,Longitude,UserId | emergency, success
| api/emergency/update/{id} | PUT | API.Emergency.Update | Updates an emergency data from the DB. | Id - Emergency ID | status |
| api/emergency/delete/{id} | DELETE | API.Emergency.DeleteConfirmed | Deletes an emergency data from the DB. | Id - Emergency ID | status, redirect |