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

{
  "FIREBASE_API_KEY": "<your Firebase key>",
  "AUTH_DOMAIN": "<your Firebase Auth domain>",
  "SERVER_BASE_URL": "https://data-organizer-server.onrender.com"
}

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

## ⚠ Firebase Authentication Deprecation Warning

The following authentication features will stop working when Firebase Dynamic Links shuts down on **25 August 2025**:

- Email link authentication for mobile apps  
- Cordova OAuth support for web apps

If you use `EMAIL_SIGNIN` (`OOB_REQ_TYPE.EMAIL_SIGNIN`) or similar features, **your users will no longer be able to sign up or sign in via email link on mobile platforms**.

Firebase’s official recommendation is to migrate to alternative methods, such as:

- Standard email/password authentication  
- OAuth2 providers (Google, Microsoft, etc.)  
- A custom backend-based authentication flow

If no action is taken, these features will continue working **only until 25 August 2025**.

More details: https://firebase.google.com/docs/auth/web/email-link-auth
