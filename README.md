# ChatGPTBasedSocialBot
Bot based on ChatGPT that allows automatic publishing of posts about offered products using the Zapier service.

## Overview
This project is a ChatGPT-based automation bot designed for generating and posting product-related content. It integrates OpenAI's API with Zapier to automate tasks such as fetching product data, generating promotional posts, and publishing them.

---

## Features
- **Product and Image Fetching:**
  - Automatically retrieves product details and associated images from predefined folders.
  - Supports random selection of product and image pairs.

- **Content Generation:**
  - Leverages OpenAI's API to generate promotional content for products.
  - Customizable prompts for tailored content creation.

- **Integration with Zapier:**
  - Posts generated content, including images, to a Zapier webhook for further automation.
  - Supports flexible integration with any platform via Zapier, allowing users to connect with tools like social media, CRMs, and more.
  - Requires setting up a Zap on Zapier to handle incoming data and trigger appropriate workflows.

- **Scheduling and Automation:**
  - Automatically schedules posts at random intervals within a specified range.
  - Ensures content is posted only when conditions are met.

---

## File Descriptions

### 1. `FileFetcher.cs`
Handles file and image retrieval for the products. It fetches data such as title, description, price, and associated image paths.

### 2. `OpenAiIntegration.cs`
Integrates with OpenAI's API to generate content based on product data. Implements retry logic for rate-limited requests.

### 3. `ZapierIntegration.cs`
Manages integration with Zapier by sending posts and associated images to a specified webhook URL.

### 4. `ConfigData.cs`
Defines the structure for configuration data, including API keys, folder paths, and webhook URLs.

### 5. `Program.cs`
The main entry point of the application. Orchestrates the overall workflow, including configuration loading, scheduling, and post automation.

### 6. `Helpers.cs`
Provides utility functions for tasks such as random date generation, configuration reading, and post date management.

---

## Setup Instructions

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/Z1EMN1AK/ChatGPTBasedSocialBot
   cd <repository-directory>
   ```

2. **Configure the Application:**
   - Create a configuration file named `ProgramConfig.cfg` in the project root.
   - Populate it with the following structure:
     ```json
     {
       "apiKey": "YOUR_OPENAI_API_KEY",
       "chatGPTModel": "text-davinci-003",
       "webhookUrl": "YOUR_ZAPIER_WEBHOOK_URL",
       "dataFolderPath": "./Data",
       "imageFolderPath": "./Images"
     }
     ```

3. **Prepare Data and Images:**
   - Place product data files in the `./Data` folder with names like `Product1.txt`, `Product2.txt`, etc.
   - Place image files in the `./Images` folder with names like `Image1.jpg`, `Image2.jpg`, etc.

4. **Set Up Zap on Zapier:**
   - Create a Zap in Zapier to handle incoming data from the webhook.
   - Configure the Zap to process and post the content to your desired platform (e.g., Facebook, Twitter, or Slack).

5. **Run the Application:**
   - Build and run the application:
     ```bash
     dotnet run
     ```
   - Follow prompts to specify the configuration file path.

---

## Usage

- The bot automatically selects a product and generates a promotional post.
- Posts are scheduled at random intervals based on the `Helpers.GetRandomDate` function.
- Generated posts are sent to a Zapier webhook for publishing.

---

## Dependencies

- .NET Core 6.0 or later
- OpenAI API
- Zapier Webhook

---

## Contributing

1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Submit a pull request with a detailed description of changes.
