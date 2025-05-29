# Description
**EncryptionService** is an educational web application focused on demonstrating and practicing core cryptographic techniques.

## 🔐 Features

- Symmetric encryption  
- Asymmetric encryption  
- Hashing  
- Digital signatures  
- Pseudo-random number generators
- Cryptanalysis tools

Built with **ASP.NET Core MVC**, the project supports encryption and decryption of both text and files in various formats.

# Technology stack
The project is developed in the **C#** programming language using **ASP.NET Core MVC** to create a modern and responsive web interface.  
It is built on **.NET 9.0**, taking advantage of the latest platform features and performance improvements.

# Installing and running the project

## Clone the repository
1. Open **Visual Studio**.
2. In the Start window, select **"Clone repository"**.
3. In the **"Repository location"** dialog, paste the following link:
   ```bash
   https://github.com/SaigoTora/EncryptionService.git
   ```
4. Select the path where the project will be cloned on your computer.
5. Click the **"Clone"** button.

## Installing dependencies
The project uses several NuGet packages. To install them:
1. Right-click the solution in **Solution Explorer**.
2. In the context menu, select **"Restore NuGet Packages"**. Visual Studio will automatically install all the necessary packages.

## Building and running the project
1. Press **Ctrl + Shift + B** to build the project.
2. Press **F5** to run the project in debug mode, or select **"Run without debugging"** (**Ctrl + F5**) for a normal run.

# Instructions for use

When you first launch the application, you will be greeted with a welcome screen and options to select the desired cryptography topic.

Available topics include:

- Symmetric encryption: using transposition ciphers  
- Symmetric encryption: using substitution ciphers  
- Symmetric encryption: use of stream ciphers (additive ciphers) and pseudorandom sequence generators  
- Asymmetric encryption  
- Hashing  
- Cryptanalysis  

Expanding each topic allows you to choose a specific algorithm or method to work with. After selecting the desired algorithm, you will be directed to a page containing input fields, action buttons, and results of the encryption or decryption operations.

Most encryption keys and related parameters are stored in the `appsettings.json` file, allowing you to modify them at any time according to your needs.

This project aims to provide a comprehensive educational tool for exploring various cryptographic methods.  
Use it responsibly and enjoy learning the fundamentals of encryption and security.
