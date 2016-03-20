@echo off
cls


robocopy paket-files\fsprojects\FsReveal . build.fsx /njh /njs /nfl /ndl
packages\FAKE\tools\FAKE.exe build.fsx %*
