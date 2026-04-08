 Education Center Management System

A comprehensive education management platform built with ASP.NET Core, Entity Framework, Clean Architecture, and CQRS, designed to efficiently manage students, teachers, sessions, bookings, attendance, and educational content with secure Access Codes.

Key Features
1️ Authentication & User Management
Register: Student / Teacher / Admin
Login / Logout
Password Reset (Email / OTP)
Email Verification
Access Code validation for first-time login
Session management and JWT Authentication
2️ User Management
 Student
Create account, view and edit profile
View session history
Progress tracking (Progress Tracking )
Teacher
Create account, view students
Manage sessions and monitor attendance
View personal statistics
 Admin
Add teachers
Full CRUD user management
Activate / deactivate accounts
Manage roles and permissions
3️Center Management
Add rooms and define capacity
Manage subjects and assign teachers
Manage timetables
4️ Sessions Management 
Create / Edit / Delete sessions
Specify: time, room, teacher, capacity, type (Online / Offline), session link (Zoom / Meet)

💥 Validation:

Prevent room or teacher conflicts
Prevent duplicate sessions
5️ Sessions Discovery & Filtering
View all sessions
Filter by subject, teacher, time, type
Advanced search and sorting by time/availability
6️ Booking System 
Book / Cancel / Rebook sessions
Automated capacity updates
Deduct from purchased package upon booking

 Rules:

Cannot book full sessions
Prevent time conflicts
Access Code required
7️ Waiting List (Smart Queue )
Join / Leave waiting list
Automatic queue management
Automatic booking when seat is available
Notification upon approval
8️ Schedule Management
 Student
View timetable, calendar view, upcoming sessions
 Teacher
View teaching schedule and manage appointments
9️ Attendance System 
Generate dynamic QR code per session
Scan QR to mark attendance
Prevent duplicate attendance
Define allowed check-in time
 Packages / Subscriptions 
Buy session packages (Basic / Premium)
Automatic discount on booking
Restrict booking when package expires
Renewal and promotions
 Notifications System 
Notifications for booking, upcoming sessions, cancellations, waiting list approval, and uploaded materials
Push and In-App notifications
Only relevant users receive notifications
Prevent duplicates
 Reports & Analytics 
 Admin
Attendance and absence statistics
Top performing teachers
Session statistics and usage analysis
Revenue tracking (optional)
Teacher
Student attendance rates
Student performance
Engagement tracking
 Materials / Content Management  (Key Feature )
 Teacher
Upload files: PDF, board images, video
Link files to a specific session
Edit / Delete materials
 Student
View session materials, download, preview (PDF / Video)

 Rules:

Students can only access booked sessions
Maximum file size restrictions
Supported file types only
Common Problems by Role
Teacher
Loss of content after session
Difficult to monitor student progress
Manual attendance tracking prone to errors
Student
Hard to organize schedule and follow-up
Difficult to access materials after session
Missed sessions or missing notifications
Admin
Hard to manage the center efficiently
Difficult to monitor performance and allocate rooms
Decision-making without proper tools can be chaotic
