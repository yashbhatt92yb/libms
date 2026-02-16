cmake_minimum_required(VERSION 3.20)

project(ABCLibraryLauncher NONE)

# CMake orchestration for a .NET WPF solution.
# This does not replace MSBuild; it wraps dotnet restore/build/publish
# so CI/CD systems that standardize on CMake can drive the project.

set(ABCLIB_SOLUTION "${CMAKE_SOURCE_DIR}/ABCLibrary.sln")
set(ABCLIB_WPF_PROJECT "${CMAKE_SOURCE_DIR}/ABCLibrary.Wpf/ABCLibrary.Wpf.csproj")

if(NOT EXISTS "${ABCLIB_SOLUTION}")
  message(FATAL_ERROR "ABCLibrary.sln not found at ${ABCLIB_SOLUTION}")
endif()

find_program(DOTNET_EXE dotnet REQUIRED)

set(ABCLIB_CONFIGURATION "Release" CACHE STRING "Build configuration")
set(ABCLIB_RUNTIME "win-x64" CACHE STRING "Publish runtime identifier")
set(ABCLIB_SELF_CONTAINED "false" CACHE STRING "Self-contained publish (true/false)")

set(ABCLIB_OUT_DIR "${CMAKE_BINARY_DIR}/publish" CACHE PATH "Publish output directory")

add_custom_target(abclibrary_restore
  COMMAND "${DOTNET_EXE}" restore "${ABCLIB_SOLUTION}"
  WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
  COMMENT "Restoring NuGet packages for ABCLibrary"
  VERBATIM
)

add_custom_target(abclibrary_build
  COMMAND "${DOTNET_EXE}" build "${ABCLIB_SOLUTION}" -c "${ABCLIB_CONFIGURATION}" -p:EnableWindowsTargeting=true
  WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
  DEPENDS abclibrary_restore
  COMMENT "Building ABCLibrary solution"
  VERBATIM
)

add_custom_target(abclibrary_publish
  COMMAND "${DOTNET_EXE}" publish "${ABCLIB_WPF_PROJECT}" -c "${ABCLIB_CONFIGURATION}" -r "${ABCLIB_RUNTIME}" --self-contained "${ABCLIB_SELF_CONTAINED}" -o "${ABCLIB_OUT_DIR}" -p:EnableWindowsTargeting=true
  WORKING_DIRECTORY "${CMAKE_SOURCE_DIR}"
  DEPENDS abclibrary_build
  COMMENT "Publishing ABCLibrary WPF executable"
  VERBATIM
)

message(STATUS "Configured ABCLibrary CMake wrapper")
message(STATUS "  Configuration    : ${ABCLIB_CONFIGURATION}")
message(STATUS "  Runtime          : ${ABCLIB_RUNTIME}")
message(STATUS "  Self-contained   : ${ABCLIB_SELF_CONTAINED}")
message(STATUS "  Publish out dir  : ${ABCLIB_OUT_DIR}")
message(STATUS "Targets available:")
message(STATUS "  abclibrary_restore")
message(STATUS "  abclibrary_build")
message(STATUS "  abclibrary_publish")
