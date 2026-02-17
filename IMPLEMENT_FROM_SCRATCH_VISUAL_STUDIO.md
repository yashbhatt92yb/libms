# ABC College Library — Build From Scratch in Visual Studio (Complete Implementation Playbook)

This document is a **hands-on implementation guide** to rebuild the application manually in Visual Studio from zero, using the current repository as your source of truth.

It includes:
- exact project creation order,
- exact file/folder structure,
- what to copy from this repo at each stage,
- package installation,
- references,
- build checkpoints,
- run and publish (`.exe`) steps.

---

## 0) Prerequisites (must be done first)

On your Windows machine install:

1. **Visual Studio 2022** (17.8+ recommended)
2. Workload: **.NET desktop development**
3. Components:
   - .NET 8 SDK
   - .NET 8 targeting pack
4. Git (optional but recommended)

If WPF project appears unloaded, missing workload/components are the primary cause.

---

## 1) Create new solution and 4 projects in Visual Studio

### 1.1 Create solution
1. Open Visual Studio
2. Create new project → **Blank Solution**
3. Solution name: `ABCLibrary`
4. Location: your chosen workspace

### 1.2 Add projects to solution (in this exact order)

1. **Class Library** → `ABCLibrary.Domain` (Target framework .NET 8)
2. **Class Library** → `ABCLibrary.Application` (Target framework .NET 8)
3. **Class Library** → `ABCLibrary.Infrastructure` (Target framework .NET 8)
4. **WPF Application** → `ABCLibrary.Wpf` (Target framework .NET 8 Windows)

Expected solution shape mirrors the repo solution. Use `ABCLibrary.sln` in repo as reference.

---

## 2) Add project references (critical)

Right-click each project → **Add Project Reference**:

- `ABCLibrary.Application` references:
  - `ABCLibrary.Domain`
- `ABCLibrary.Infrastructure` references:
  - `ABCLibrary.Domain`
  - `ABCLibrary.Application`
- `ABCLibrary.Wpf` references:
  - `ABCLibrary.Domain`
  - `ABCLibrary.Application`
  - `ABCLibrary.Infrastructure`

---

## 3) Install NuGet packages

Install these package versions exactly:

### 3.1 ABCLibrary.Application
- `BCrypt.Net-Next` `4.0.3`
- `ClosedXML` `0.104.2`

### 3.2 ABCLibrary.Infrastructure
- `Microsoft.EntityFrameworkCore` `8.0.8`
- `Microsoft.EntityFrameworkCore.Sqlite` `8.0.8`
- `Microsoft.EntityFrameworkCore.Design` `8.0.8`
- `ZXing.Net` `0.16.9`
- `System.Drawing.Common` `8.0.8`

### 3.3 ABCLibrary.Wpf
- `CommunityToolkit.Mvvm` `8.2.2`
- `MaterialDesignThemes` `5.1.0`
- `MaterialDesignColors` `3.0.0`

Tip: compare your generated `.csproj` with repo `.csproj` files after package installation.

---

## 4) Copy files project-by-project from this repository

> Use this repo as source. Create identical folders/files and paste contents.

---

## 4A) Domain project files to copy

Create folders:
- `Entities`
- `Enums`

Copy these files from repo:

- `ABCLibrary.Domain/Entities/BaseEntity.cs`
- `ABCLibrary.Domain/Entities/User.cs`
- `ABCLibrary.Domain/Entities/Book.cs`
- `ABCLibrary.Domain/Entities/BookCopy.cs`
- `ABCLibrary.Domain/Entities/Student.cs`
- `ABCLibrary.Domain/Entities/LibraryTransaction.cs`
- `ABCLibrary.Domain/Entities/SystemLog.cs`
- `ABCLibrary.Domain/Entities/SystemSetting.cs`
- `ABCLibrary.Domain/Entities/BarcodeReprintLog.cs`
- `ABCLibrary.Domain/Enums/TransactionStatus.cs`

✅ **Checkpoint build**: Build only `ABCLibrary.Domain` (should succeed).

---

## 4B) Application project files to copy

Create folders:
- `DTOs`
- `Interfaces`

Copy these files from repo:

- `ABCLibrary.Application/DTOs/AuthDtos.cs`
- `ABCLibrary.Application/DTOs/BookDtos.cs`
- `ABCLibrary.Application/DTOs/CirculationDtos.cs`
- `ABCLibrary.Application/DTOs/ReportDtos.cs`
- `ABCLibrary.Application/Interfaces/IRepository.cs`
- `ABCLibrary.Application/Interfaces/IUnitOfWork.cs`
- `ABCLibrary.Application/Interfaces/IServices.cs`
- `ABCLibrary.Application/Interfaces/ILabelPrinterService.cs`

✅ **Checkpoint build**: Build `Domain` + `Application`.

---

## 4C) Infrastructure project files to copy

Create folders:
- `Data`
- `Repositories`
- `Services`

Copy these files from repo:

- `ABCLibrary.Infrastructure/Data/LibraryDbContext.cs`
- `ABCLibrary.Infrastructure/Data/DbBootstrapper.cs`
- `ABCLibrary.Infrastructure/Repositories/Repository.cs`
- `ABCLibrary.Infrastructure/Repositories/UnitOfWork.cs`
- `ABCLibrary.Infrastructure/Services/LoggingService.cs`
- `ABCLibrary.Infrastructure/Services/AuthService.cs`
- `ABCLibrary.Infrastructure/Services/BarcodeService.cs`
- `ABCLibrary.Infrastructure/Services/BookService.cs`
- `ABCLibrary.Infrastructure/Services/StudentService.cs`
- `ABCLibrary.Infrastructure/Services/CirculationService.cs`
- `ABCLibrary.Infrastructure/Services/ReportService.cs`
- `ABCLibrary.Infrastructure/Services/BackupService.cs`
- `ABCLibrary.Infrastructure/Services/TscLabelPrinterService.cs`
- `ABCLibrary.Infrastructure/DependencyInjection.cs`

✅ **Checkpoint build**: Build `Domain` + `Application` + `Infrastructure`.

---

## 4D) WPF project files to copy

Create folders:
- `Commands`
- `Styles`
- `ViewModels`
- `Views`

Copy these files from repo:

- `ABCLibrary.Wpf/App.xaml`
- `ABCLibrary.Wpf/App.xaml.cs`
- `ABCLibrary.Wpf/Commands/AsyncRelayCommand.cs`
- `ABCLibrary.Wpf/Styles/Theme.xaml`
- `ABCLibrary.Wpf/ViewModels/ViewModelBase.cs`
- `ABCLibrary.Wpf/ViewModels/LoginViewModel.cs`
- `ABCLibrary.Wpf/ViewModels/MainViewModel.cs`
- `ABCLibrary.Wpf/Views/LoginWindow.xaml`
- `ABCLibrary.Wpf/Views/LoginWindow.xaml.cs`
- `ABCLibrary.Wpf/Views/MainWindow.xaml`
- `ABCLibrary.Wpf/Views/MainWindow.xaml.cs`

✅ **Checkpoint build**: Build entire solution.

---

## 5) Validate WPF project settings if project unloads

Open `ABCLibrary.Wpf.csproj` and ensure:

- `Sdk="Microsoft.NET.Sdk.WindowsDesktop"`
- `<TargetFramework>net8.0-windows</TargetFramework>`
- `<UseWPF>true</UseWPF>`
- `<OutputType>WinExe</OutputType>`

If still unloaded:
- Visual Studio Installer → Modify → add `.NET desktop development`.

---

## 6) Remove template leftovers and keep structure clean

When creating fresh projects, Visual Studio may generate default files (`Class1.cs`, default `MainWindow` etc.).

Delete/replace defaults so your tree matches repo implementation.

Use repo root file listing as reference:

- `ABCLibrary.Domain/*`
- `ABCLibrary.Application/*`
- `ABCLibrary.Infrastructure/*`
- `ABCLibrary.Wpf/*`

---

## 7) Set startup and run

1. Right click `ABCLibrary.Wpf` → **Set as Startup Project**
2. Run (`F5`)
3. App should initialize SQLite DB at:
   - `C:\ProgramData\ABCLibrary\library.db`
4. Login credentials:
   - Username: `admin`
   - Password: `Admin@123`

If login window appears, startup wiring and DI are working.

---

## 8) Create release EXE (publish)

### Option A — Visual Studio Publish wizard
1. Right click `ABCLibrary.Wpf` → **Publish**
2. Target: **Folder**
3. Configuration: `Release`
4. Runtime: `win-x64`
5. Choose one:
   - Framework-dependent (smaller)
   - Self-contained (easier deployment)

### Option B — CLI (from solution folder)

Framework-dependent:
```powershell
dotnet publish ABCLibrary.Wpf/ABCLibrary.Wpf.csproj -c Release -r win-x64 --self-contained false
```

Self-contained:
```powershell
dotnet publish ABCLibrary.Wpf/ABCLibrary.Wpf.csproj -c Release -r win-x64 --self-contained true
```

Single-file self-contained (optional):
```powershell
dotnet publish ABCLibrary.Wpf/ABCLibrary.Wpf.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Published output folder contains your executable.

---

## 9) Recommended build checkpoints while implementing

After each stage, run Build Solution:

1. After Domain copy
2. After Application copy
3. After Infrastructure copy
4. After WPF copy
5. After package install updates

This isolates errors quickly.

---

## 10) Error-to-fix quick table

### Error: `ABCLibrary.Wpf (unloaded)`
Fix: install `.NET desktop development` workload + .NET 8 targeting pack.

### Error: `Microsoft.NET.Sdk.WindowsDesktop not found`
Fix: same as above; ensure project targets `net8.0-windows`.

### Error: missing ZXing renderer type
Fix: ensure current `BarcodeService.cs` from repo is used (PixelData implementation).

### Error: DB not created
Fix: verify app can write to `C:\ProgramData\ABCLibrary` and startup reaches `DbBootstrapper.InitializeAsync`.

---

## 11) Optional CMake wrapper usage (if your org uses CMake)

This repo includes `CMakeLists.txt` that wraps dotnet commands:

```powershell
cmake -S . -B build
cmake --build build --target abclibrary_restore
cmake --build build --target abclibrary_build
cmake --build build --target abclibrary_publish
```

Use this as orchestration; actual compilation is still dotnet/MSBuild-based.

---

## 12) Practical implementation sequence (best practice)

To avoid confusion, do this exact sequence:

1. Create solution/projects
2. Add project references
3. Install NuGet packages
4. Copy Domain files
5. Build
6. Copy Application files
7. Build
8. Copy Infrastructure files
9. Build
10. Copy WPF files
11. Build
12. Run app
13. Publish EXE

If you follow this order, you can rebuild the same solution from scratch with minimal issues.

---

## 13) File source of truth

While implementing manually, always copy from **these existing repo files** (current state):

- `ABCLibrary.Domain/**`
- `ABCLibrary.Application/**`
- `ABCLibrary.Infrastructure/**`
- `ABCLibrary.Wpf/**`
- `README.md`
- `VISUAL_STUDIO_RUN_GUIDE.md`

This ensures your manually recreated solution stays exactly aligned with this repository.
