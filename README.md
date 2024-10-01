# cis-api-legacy-integration-phase-2

## General Description

---

In this phase, the focus is on implementing the Crowdsourced Ideation Solution (CIS) API, which extends the functionality established in Phase 1. This phase aims to develop and integrate a modern API for the CIS platform, allowing users to create topics, submit ideas, and vote on them. The solution must seamlessly interact with the legacy system via the user API created in Phase 1; the primary goal is to ensure that the CIS API is robust, functional, and aligned with the overall project objectives while providing a user-friendly interface for interacting with topics and ideas.

![phase2.png](public/img/phase2.png)

## Prerequisites

---

- The user API must be fully developed and operational, providing necessary endpoints for user authentication and data management.
- The Java-based user API must be functional and integrated with the existing system to support interactions with the CIS API.
- The existing database schema must be extended to accommodate topics and ideas, ensuring proper data storage and retrieval.
- A simple client application must be available for testing and validating the CIS API operations, including CRUD functionality for topics and ideas.

## Execute API in code first
### Steps for run code C#
### step 1
Modify the environment variables for the database in the appsetting.json file
```
"MySqlConnection": "server={HOST};port={PORT};database={NAME-DATABASE};uid={USERNAME};password={PASSWORD};"
```
### step 2
Run the command for nuget package manager
```
Update-Database
```
or in console
```
dotnet ef database update
```
