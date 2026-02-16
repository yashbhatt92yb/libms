# ABC College Library Management System

Production-oriented single-user desktop Library Management System for librarians.

## Stack
- .NET 8
- WPF + MVVM
- EF Core + SQLite (`C:\ProgramData\ABCLibrary\library.db`)
- BCrypt password hashing
- ZXing.Net Code 128 barcode generation
- TSPL printer command output for TSC TTP-244 Pro
- ClosedXML report export

## Modules Implemented
- Authentication with hashed password validation and login logging.
- Dashboard summary cards.
- Book management with multi-copy + immutable barcode per copy (`LIB-0000001`).
- Student management and active/inactive handling model.
- Issue/return workflows with transaction scope and fine calculation.
- Barcode reprint logging.
- Inventory report export to Excel.
- Manual backup service for SQLite database.
- System logging model + persistence.

## First Run
- Database folder is created automatically.
- Database is initialized on startup.
- Default admin user:
  - Username: `admin`
  - Password: `Admin@123`

## Notes
- EF migrations can be added with `dotnet ef` in a Windows/.NET SDK environment.
- Current UI is modernized with MaterialDesign themes and can be extended into dedicated module pages.

## Visual Studio Run Guide
- See `VISUAL_STUDIO_RUN_GUIDE.md` for complete build/run/validation/publish instructions on Windows + Visual Studio.
