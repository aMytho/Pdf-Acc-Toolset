# PDF Accessibility Toolset
A web application to help make PDFs accessible. Built with [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) and [iText](https://itextpdf.com/).
The project can be accessed on [our website](https://pdf-accessibility.tools).

![Toolbox Demo](https://github.com/aMytho/Pdf-Acc-Toolset/assets/58316242/7f90bd11-3cc1-4f9a-8728-1d2f6994947a)


## Purpose

This application exists to improve the PDF accessibility process. Many operations involving the tag tree are *very* repetitive. This application removes a lot of the manual work that was previously done with a tool like Adobe Acrobat.

This is a free and open source app. It can run in your browser without any installation. All PDF modification is done locally on the client machine. You shouldn't need to pay a fee to make a document accessible. This project will help to fix that issue.

## Features

While the end goal is to remove Acrobat (and any paid tool) from the accessibility workflow entirely, we have a *long* ways to go. At the moment, the project focuses on removing the operations that take the most time. The current features are as follows:

- Set basic accessibility metadata.
- List Generation: Generates a proper list with all the required tags. Useful for when you have lists that were detected as individual paragraphs.
- Table Generation: Generates a proper table with a specified amount of rows and columns. The first row are automatically TH elements.
- Tag Generation: Generate any amount of a specified tag. Unlike the above options, this **only** generates the specified tag, not any children of the tag. For example, generating 50 lists with the Tag Generation tool will not generate the required LI, Lbl, and LBody tags. Use the above tools for those cases!
- Attribute Modifier: Allows for batch tag modification. You can modify the ID, Title, Alt Text, and Actual text.

![Task Queue](https://github.com/aMytho/Pdf-Acc-Toolset/assets/58316242/907eb38b-eb18-421a-acd2-8cd2fae5e56b)

## Documentation
The project docs are hosted on a separate website. They can be accessed [here](https://docs.pdf-accessibility.tools/docs)

## Installation for Development

> This is only neccessary if you want to develop the project. If you just want to use the application, head to [our website](https://pdf-accessibility.tools).

You will need the [.NET 8 SDK and Runtime Environment](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) as well as the corresponding ASP release. To generate the CSS you will an need an LTS build of [NodeJS/NPM](https://nodejs.org/en).

> Getting the correct version of .NET can be difficult. If you are not sure what to install, download the SDK for 8.0.x. Run it. Once completed, restart your PC. Continue with the below instructions. When you get to running the application, .NET will provide the links to the exact versions you need.
> The same process applies if you already have a .NET SDK or Runtime installed. Open a terminal and run `dotnet --list-sdks` or `dotnet list-runtimes` to see the installed versions.

### Download the Project

First, download the source code. You can download the zip using the green "Code" button at the top of this page or use a git command `git clone https://github.com/amytho/pdf-acc-toolset`

### Install Dependencies

Next, install iText and the other dependencies: `dotnet restore`

To generate the styles you will need to install the NodeJS dependencies: `npm install`

### Build the CSS

This project uses TailwindCSS. As a result, you need to generate the corresponding CSS files.
Run the following command: `npm run build`. This will generate an `output.css` file in the `wwwroot/css` directory.

If you intent to make changes to the CSS, run `npm run dev`. This will automatically recompile when a file is updated.

> The first time you run this command it will ask you to install the Tailwind NPX executable.

### Run the Project

Run the command to start the project.

`dotnet watch`

This will start the project in development mode. Your browser will be opened to `localhost:5005`. You can now use the project!

> You may be asked to trust the ASP certificate. This is only required if you want to use the HTTPS version. There is no functional difference between the two since all PDF modification is done client-side.

## Feature Requests

Let me know if you have any suggestions. This is just a side project to help myself at work, but anyone is welcome to suggest improvements or contribute directly!

## License

This software is under the AGLPLv3 license as it uses the community edition of the iText library.
This project does not modify the iText software in any way.
Their license can be seen on the [iText License](https://itextpdf.com/how-buy/legal/agpl-gnu-affero-general-public-license) page.
