# PDF-Acc-Toolset
A web application to help make PDFs accessible. Built with [Blazor](https://dotnet.microsoft.com/en-us/apps/aspnet/web-apps/blazor) and [iText](https://itextpdf.com/).

## Purpose

The main app to make PDFs accessible is Adobe Acrobat Pro DC. However, this app is full of bugs, inefficient workflows, and costs far too much for what is is. Additionally, there are little to no alternatives. While other PDF editors exist, none support accessibility options aside from a [CommonLook Plugin for Acrobat](https://commonlook.com/accessibility-software/pdf/) and a [cloud based](https://equidox.co/pdf-solutions/pdf-accessibility-software/) platform (Equidox). Both of these are valid options, but they don't list a price on their website. Not good!

You shouldn't need to pay a fee to make a document accessible. This project will help to fix that issue.

## Features

While the end goal is to remove Acrobat from the accessibility workflow entirely, we have a *long* ways to go. At the moment, the project focuses on removing the operations that take the most time. The current features are as follows:

- List Generation: Generates a proper list with all the required tags. Useful for when you have lists that were detected as individual paragraphs.
- Table Generation: Generates a proper table with a specified amount of rows and columns. The first row are automatically TH elements.
- Tag Generation: Generate any amount of a specified tag. Unlike the above options, this **only** generates the specified tag, not any children of the tag. For example, generating 50 lists with the Tag Generation tool will not generate the required LI, Lbl, and LBody tags. Use the above tools for those cases!

## Installation

At the moment, you must build the project yourself. You will need the .NET 7 (SDK and Runtime) as well as the corresponding ASP release. I will switch to using Github Pages once I can confirm the project is stable.

A proper dev guide will also be included when the project is stable.

## Usage

Select the upload button and choose a PDF. Once the PDF is loaded, begin using the tools to make modifications. Next, visit the Tasks page to see all queued changes. Finally, head to the Save page and download your new PDF.

## Keyboard Shortcuts

*These need to be migrated from the prototype in Windows Forms...*

## Feature Requests

Let me know if you have any suggestions. This is just a side project to help myself at work, but anyone is welcome to suggest improvements or contribute directly!

## License

This software is under the AGLPLv3 license as it uses the community edition of the iText library.
This project does not modify the iText software in any way.
Their license can be seen on the [iText License](https://itextpdf.com/how-buy/legal/agpl-gnu-affero-general-public-license) page.
