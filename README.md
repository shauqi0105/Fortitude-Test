---

## 📦 Features

| Feature                      | Description                                                   |
|------------------------------|---------------------------------------------------------------|
| ✅ Partner Authentication     | Verifies `partnerrefno`, `partnerkey`, and base64 password     |
| ✅ Signature Validation       | Uses SHA-256 hashing and Base64 encoding for request signing   |
| ✅ Discount Calculation       | Applies multi-rule logic, capped at 20%                        |
| ✅ Input Validation           | Checks all required fields and returns clear error messages    |
| ✅ Logging with log4net       | Logs requests/responses and errors, masks sensitive data        |
| ✅ Swagger Enabled            | Auto-generates interactive API docs                            |
| ✅ Docker Support             | Runs fully in a container with no external dependencies         |

---

## 🛠 Tech Stack

- ASP.NET Core 6.0
- C#
- xUnit (unit tests)
- log4net (logging)
- Docker (containerization)
- Swagger / OpenAPI (API docs)

---

## 🗂 Project Structure

---

## 🧪 Sample API Request

**POST** `/api/submittrxmessage`

```json
{
  "partnerkey": "FAKEGOOGLE",
  "partnerrefno": "FG-00001",
  "partnerpassword": "RkFLRVBBU1NXT1JEMTIzNA==",
  "totalamount": 1000,
  "timestamp": "2024-08-15T02:11:22.0000000Z",
  "sig": "VALID_GENERATED_SIGNATURE"
}

{
  "result": 1,
  "totalamount": 1000,
  "totaldiscount": 0,
  "finalamount": 1000
}

Expected response:
{
  "result": 1,
  "totalamount": 1000,
  "totaldiscount": 0,
  "finalamount": 1000
}


Running the Project with Docker
Step 1: Build Docker Image
Open a terminal in the root folder:
docker build -t fortitude-api .

Step 2: Run Docker Container
docker run -d -p 8080:80 fortitude-api

Step 3: Access the API
Open Swagger in your browser:
http://localhost:8080/swagger
