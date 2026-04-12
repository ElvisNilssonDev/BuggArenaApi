<div align="center">

<img src="https://img.shields.io/badge/-.NET_10-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
<img src="https://img.shields.io/badge/-React_19-61DAFB?style=for-the-badge&logo=react&logoColor=black" />
<img src="https://img.shields.io/badge/-SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white" />
<img src="https://img.shields.io/badge/-JWT-000000?style=for-the-badge&logo=jsonwebtokens&logoColor=white" />
<img src="https://img.shields.io/badge/-Vite-646CFF?style=for-the-badge&logo=vite&logoColor=white" />
<img src="https://img.shields.io/badge/-EF_Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />

<br/><br/>

# 🐛 BugArena

### Debug. Compete. Dominate.

**BugArena is a competitive debugging platform where developers submit
buggy code challenges, race each other to find the fix, and earn points
based on speed, accuracy, and creativity.**

<br/>

[Features](#-features) · [Tech Stack](#-tech-stack) · [Quick Start](#-quick-start) · [Points System](#-points-system) · [Installation Guide](#-installation-guide)

<br/>

</div>

---

## 🎯 What is BugArena?

Most developers spend more time debugging than writing new code — so why
not make it a sport?

BugArena is a platform built around that idea. Users craft challenges by
submitting real, intentionally broken code. Others dive in, hunt down the
bug, submit their fix, and get scored. The first person to solve a
challenge earns double points. Solve it in under five minutes and you get
a speed bonus on top of that.

The result is a leaderboard-driven community where debugging skill is
something you can actually show off.

---

## ✨ Features

| | Feature | Description |
|---|---|---|
| 🔐 | **Auth** | Register and log in with email and password. JWT tokens handle sessions automatically |
| 🐛 | **Challenges** | Browse all challenges with filters for language, difficulty, and keyword search |
| ✏️ | **Create** | Submit your own buggy code with expected vs actual behavior and optional hints |
| 🔧 | **Solve** | Submit a fix with an explanation. Your time-to-solve is tracked from the moment you open the challenge |
| ✅ | **Review** | As a challenge author, approve or reject incoming solutions directly from your profile |
| 🏆 | **Leaderboard** | All-time global rankings and a weekly reset competition, with a podium for the top 3 |
| 👤 | **Profile** | Personal stats, challenges you've created, and a queue of solutions waiting for your review |
| 🛡️ | **Admin Panel** | Manage every user on the platform — promote to admin, revoke access, or delete accounts |

---

## 🛠 Tech Stack

### Backend — [BugArenaApi](https://github.com/ElvisNilssonDev/BugArenaApi)
- **ASP.NET Core 10** — REST API with Clean Architecture
- **Entity Framework Core 10** — Code-first migrations, SQL Server
- **FluentValidation** — Structured request validation
- **JWT Bearer Auth** — Stateless authentication with role-based authorization

### Frontend — [BugArena](https://github.com/ElvisNilssonDev/BugArena)
- **React 19** + **Vite 8** — Fast SPA with a custom client-side router
- **Axios** — HTTP client with automatic JWT injection
- **Vanilla JS/JSX** — No UI library, fully hand-crafted components and hooks

---

## ⚡ Quick Start

### Backend

```bash
git clone https://github.com/ElvisNilssonDev/BugArenaApi.git
cd BugArenaApi

# Apply database migrations
dotnet ef database update \
  --project src/BugArena.Infrastructure \
  --startup-project src/BugArena.API

# Start the API
dotnet run --project src/BugArena.API
```

API runs at `http://localhost:5159`  
Swagger UI at `http://localhost:5159/swagger`

### Frontend

```bash
git clone https://github.com/ElvisNilssonDev/BugArena.git
cd BugArena
npm install
npm run dev
```

App runs at `http://localhost:5173`

> **Note:** Always start the backend before the frontend.

---

## 🏅 Points System

Points are calculated and awarded automatically the moment a solution
gets approved.

---

## 📥 Installation Guide

A complete installation guide is available as a downloadable document.
It covers all prerequisites, configuration files, database setup,
creating your first admin account, and a troubleshooting section for
common issues.

<div align="center">

### [📥 Download Installation Guide (.docx)](./BugArena-Installation-Guide.docx)

</div>

---

<div align="center">

Built with ☕ by Elvis & Benji and Hashi · 2026

</div>
