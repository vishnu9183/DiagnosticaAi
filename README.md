Diagnostica Healthcare Management System
Diagnostica is a comprehensive healthcare management system that seamlessly integrates advanced AI capabilities into a native full-stack application. Built with .NET and Blazor for the front-end and back-end—and enhanced with Python Flask microservices for AI using the Gemini Flash 2.0 model—Diagnostica streamlines patient management, appointment scheduling, and diagnostic insights. The system uses Google Cloud PostgreSQL for secure and scalable data storage.

Link to Demo - https://www.linkedin.com/posts/activity-7313369373748994049-01Pf?utm_source=share&utm_medium=member_desktop&rcm=ACoAADEwR2kBiQgcJFfgSac1eg1K1OxaYS5akc0

Table of Contents
Features

Tech Stack

Architecture Overview

Installation

Usage

API Endpoints

Contributing

License

Features
Patient Management:

User registration and login

Profile management including health history and notifications

Appointment scheduling and file uploads for AI diagnostic analysis

Doctor Portal:

Doctor login and profile management

Appointment management: view, accept, or reject appointments

Access to patient profiles and medical history

Admin Dashboard:

Manage (add, delete, update) both patient and doctor records

Visualize key metrics and trends through dashboards

AI & LLM Integration:

A Python Flask microservice leverages the Gemini Flash 2.0 model for real-time image analysis and symptom prediction

Enhance diagnostic accuracy by integrating advanced AI into an existing native web application

Database:

Google Cloud PostgreSQL for secure, scalable data management

Tech Stack
Frontend & Backend: .NET 6+ / Blazor Server, C#

AI & Microservices: Python, Flask, Gemini Flash 2.0

Database: Google Cloud PostgreSQL

UI Components: MudBlazor

Architecture Overview
Diagnostica is designed as a modular system where the core web application (developed in .NET/Blazor) communicates with external Python microservices via RESTful APIs. 
This architecture allows you to integrate state-of-the-art AI/LLM features without overhauling your existing system.

Installation
Prerequisites
.NET 6 SDK or later

Python 3.8+

Google Cloud PostgreSQL

Package managers (NuGet for .NET, pip for Python)

MudBlazor

Usage
Patients: Log in, manage profiles, schedule appointments, and upload scans for AI analysis.

Doctors: Log in, manage appointments (accept/reject), and view patient details.

Admins: Manage patient and doctor records and view dashboard visualizations for key metrics.

API Endpoints
Patients
POST /api/patients/login – Patient login

GET /api/patients/{id} – Retrieve patient details

Doctors
POST /api/doctors/login – Doctor login

GET /api/doctors – Retrieve approved doctors

POST /api/doctors – Add a new doctor

DELETE /api/doctors/{id} – Delete a doctor

GET /api/doctors/specialty/{specialty} – Retrieve doctors by specialty

GET /api/doctors/{id} – Retrieve doctor details

Appointments
POST /api/appointments – Create an appointment

GET /api/appointments/{id} – Retrieve appointment details

PUT /api/appointments/{id}/accept – Accept an appointment

PUT /api/appointments/{id}/reject – Reject an appointment

PUT /api/appointments/{id}/patientnotes – Update patient notes

PUT /api/appointments/{id}/doctornotes – Update doctor notes

PUT /api/appointments/{id}/aioutput – Update AI output



