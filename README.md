# ✍️ SignAI

**Generate clean, professional email signatures using AI — all by sending an email.**

> Built with 💙 using Azure Functions, Azure OpenAI, C#, and Postmark.

---

## 📬 What Is It?

**SignAI** is an AI-powered service that creates minimalist, responsive HTML email signatures based on user input — all via email.
Send your contact info and preferences to a dedicated email address, and SignAI will reply with ready-to-use signatures.

This project is created for the dev.to challenge: https://dev.to/devteam/join-the-postmark-challenge-inbox-innovators-3000-in-prizes-497l?bb=232850

---

## 🚀 Features

* ✉️ **Email-based UX** – no forms, just send an email
* 🧠 **AI-generated HTML signatures** – minimal, accessible, and stylish
* 🎨 **Supports preferences** – dark/light style, emojis, links, layout tweaks
* 💡 **Multiple themes (soon)** – choose minimalist, colorful, corporate, etc.
* 🖼️ **Preview-ready** – includes rendered image preview for quick testing

---

## 🛠 Tech Stack

| Component               | Technology Used                                                                         |
| ----------------------- | --------------------------------------------------------------------------------------- |
| 💌 Email Handling       | [Postmark Inbound](https://postmarkapp.com)                                             |
| 🧠 AI Generation        | [Azure OpenAI Service](https://azure.microsoft.com/products/cognitive-services/openai/) |
| ⚙️ Backend API          | Azure Functions (C#)                                                                    |
| ☁️ Cloud Platform       | Azure App Services + Storage                                                            |
| 🧑‍💻 Development Tools | Visual Studio Code, GitHub Copilot                                                      |

---

## 📥 How It Works

1. **Send an email** to `a274c047ed8bf5fe0171766e9e428d3a@inbound.postmarkapp.com` with your info:

   ```
   Name: Emanuele Bartolesi
   Role: Senior Cloud Engineer at Xebia
   Website: https://emanuelebartolesi.com
   LinkedIn: /in/kasuken
   GitHub: https://github.com/kasuken
   Phone: +41 23 3214 43
   Pronouns: he/him
   Style: Minimalist, monochrome
   ```

2. 🧠 **Azure OpenAI** generates a responsive HTML signature using GPT.

3. 📤 You get a reply with:

   * HTML code ready for Gmail/Outlook
   * Inline preview
   * Optional download link or vCard

---

## 🧪 Local Development

1. **Clone the repo**

   ```bash
   git clone https://github.com/kasuken/SignAI.git
   cd SignAI
   ```

2. **Set environment variables**

   You'll need:

   * Postmark Inbound webhook secret
   * Azure OpenAI API key
   * Your preferred OpenAI model (e.g., `gpt-4` or `gpt-35-turbo`)
   * Azure Function local settings file (`local.settings.json`)

3. **Run locally**

   ```bash
   func start
   ```

---

## 🧠 Prompt Engineering

We use a system prompt like:

> *"You are a professional HTML designer who creates minimalist, responsive, accessible email signatures in monochrome. Use table layouts and inline styles. Always reply with HTML only. Use the user's info and preferences to customize the signature."*

---

## 🤖 GitHub Copilot & VS Code

This project is built with the help of:

* GitHub Copilot for rapid iteration
* Visual Studio 2022 and JetBrains Rider

---

## 📄 License

[MIT License](LICENSE)

---

## 💬 Want to Contribute?

This is a hackathon project, but I’d love to hear your feedback or feature ideas. Feel free to open issues or submit pull requests!

---

## 🙌 Acknowledgements

* [Postmark](https://postmarkapp.com)
* [Azure OpenAI](https://learn.microsoft.com/en-us/azure/cognitive-services/openai/)
* [Azure Functions](https://learn.microsoft.com/en-us/azure/azure-functions/)
