# 📱 Data Organizer – AI-powered Mobile App for Audio Summarization

**Data Organizer** is a cross-platform mobile application built with .NET MAUI. It enables users to transcribe real-time speech and automatically generate short summaries using AI. It supports cloud storage, authentication, and import/export of files.

---

## 📦 Requirements

- [Visual Studio 2022+](https://visualstudio.microsoft.com/) with workloads:
  - .NET Multi-platform App UI development
  - .NET Desktop Development
- [Git](https://git-scm.com/downloads) - to clone the repository.

---

## 📥 Cloning the Repository

To download the project, clone it using Git:

"git clone https://github.com/Denis-Bredun/Data-Organizer"

Then open the solution:

"Data_Organizer.sln"

---

## 📱 Running the Application on a Physical Android Device

1. **Enable Developer Mode on your Android device**:
   - Go to **Settings > About phone**
   - Tap **Build number** 7 times until Developer Mode is enabled

2. **Enable USB Debugging**:
   - Go to **Settings > Developer options**
   - Enable **USB Debugging**

3. **Connect your device to the PC via USB**

4. **Open the solution**: `Data_Organizer.sln`

5. In **Visual Studio**:
   - Select your physical device in the run target dropdown
   - Click **Rebuild** the solution to restore all dependencies
   - Press **F5** or click **Start** to launch the application on the device

---

## ⚙ Configuration

The app uses the file:

"Data_Organizer/appsettings.json"

Example:
```json
{
  "FIREBASE_API_KEY": "<your Firebase key>",
  "AUTH_DOMAIN": "<your Firebase Auth domain>",
  "SERVER_BASE_URL": "https://data-organizer-server.onrender.com"
}
```

### Explanation:
- `FIREBASE_API_KEY` and `AUTH_DOMAIN` are from your Firebase project's Web App settings.
- `SERVER_BASE_URL` is the URL of the current deployed server on Render.com.

---

## 🧪 Running Tests

1. **Open the solution** `Data_Organizer.sln` in Visual Studio 2022.

2. **Open the Test Explorer**:
   - Go to **Test** → **Test Explorer** (or press `Ctrl+E, T`).

3. In the **Test Explorer**, you will see all tests from the `Data_Organizer.Tests` project.

4. To run tests:
   - Click **Run All** to execute all tests,
   - Or select specific test(s) and click **Run Selected Tests**.

5. Test results will appear in the **Test Explorer** window, showing passed, failed, and skipped tests.

---

## ⚠Firebase Authentication Deprecation Warning

Due to the shutdown of Firebase Dynamic Links on **25 August 2025**, email link authentication for mobile apps and Cordova OAuth support for web apps will no longer function. Users relying on these features will be unable to sign up or sign in via email link on mobile platforms after this date. Firebase recommends migrating to alternative authentication methods such as standard email/password, OAuth2 providers (Google, Microsoft, etc.), or a custom backend-based authentication flow to ensure uninterrupted service.

More details: 
https://firebase.google.com/support/dynamic-links-faq?hl=en#impacts-on-email-link-authentication
https://cloud.google.com/identity-platform/docs/reference/rest/v1/OobReqType
https://firebase.google.com/docs/auth/web/email-link-auth

--- 

### 🛠️ Continuous Integration

![CI Status](https://github.com/Denis-Bredun/Data-Organizer/actions/workflows/maui-ci.yml/badge.svg)

This project uses **GitHub Actions** for Continuous Integration (CI).  
On every push to the `master` branch:

- ✅ The Android `.apk` is automatically built  
- 🔐 A signed `.apk` file is uploaded as a build artifact  
- 🧪 Unit tests are executed to verify application logic

You can find recent builds and test results in the [Actions tab](https://github.com/Denis-Bredun/Data-Organizer/actions).

> Workflow config: [.github/workflows/maui-ci.yml](.github/workflows/maui-ci.yml)
