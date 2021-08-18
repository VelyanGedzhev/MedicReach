# :hospital: MedicReach
A web application which enables patients to book medical appointments with ease. 
Two types of profiles can be created depending on the role - either as a regular user or as a healthcare professional. During sign up all relevant details about the person are collected.

## Table of Contents
* [General Info](#information_source-general-information)
* [Setup](#bulb-setup)
* [Technologies Used](#technologies-used)
* [Room for Improvement](#room-for-improvement)

 ## License

This project is licensed under the [MIT License](LICENSE).


## :information_source: General Information

**Guests**
- Can check all currently available Medical Centers and Physicians

**Users**
- Can check all currently available Medical Centers and Physicians
- Can complete their profile by choosing their role - Patient or Physician

**Patients**
- Can Edit their profiles
- Can book an appointment, which has to be approved by the chosen Physician
- Can write a review after their examination *(appointment must be accepted and the appointment mustn't be reviewed yet)* .

**Physicians**
- After registration as a Physician it must be approved by Administrator to become available for other Users
- Can Edit their profiles, which changes their status to unapproved again
- Can create a Medical Center and set it's joining code
- Can join already existing Medical Center by providing it's joining code
- Can edit their current Medical center if they are it's creator
- Can approve or disapprove booked appointments

**Administrator**
- Can approve or disapprove Physicians
- Can edit Physicians and Medical Centers
- Can add new Physician Specialities, Medical Center Types, Cities and Countries

## :bulb: Setup
When starting for a first time a sample account for each role will be seeded:
- Patient -> Email: patient@medicReach.com / Password: 123456 
- Physician -> Email: physician@medicReach.com / Password: 123456 
- Administrator -> Email: admin@medicReach.com / Password: admin123456 

**:heavy_exclamation_mark: Important - appointments are in Universal Time Coordinated(UTC)!**

## Technologies Used

- ASP.NET Core 5
- Entity Framework Core 5.0.9
- Microsoft SQL Server
- AutoMapper
- MyTested.AspNetCore.Mvc
- Fluent Assertions

## Room for Improvement

**TO DO:**
- More User-Friendly way of creating new Medical Center while Physician profile is created
- Option to choose between the default and custom working schedule (both for working days and hours)