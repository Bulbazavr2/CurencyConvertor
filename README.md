# Currency Converter

A web-based application for converting amounts between various currencies using real-time exchange rates provided by the Central Bank of Russia.

## Features

- Fetches the latest exchange rates from the Central Bank of Russia.
- Supports conversion between multiple currencies, including the Russian Ruble (RUB) as the base currency.
- Dark-themed, responsive user interface for a seamless user experience.
- Allows quick switching between source and target currencies.
- Built using modern web technologies.

## Project Structure

- **Backend**:
  - **ASP.NET Core**: Manages API endpoints for fetching currency rates and performing conversions.
  - **CurrencyController.cs**: Handles API requests and processes data from the Central Bank of Russia's XML API.
  - **CurrencyDto.cs**: Defines the structure for currency data (code and name).
  - **Program.cs**: Configures the web application and sets up CORS policy.

- **Frontend**:
  - **HTML/CSS**:
    - **index.html**: Provides the structure of the web application.
    - **styles.css**: Implements a dark-themed design with responsive layouts.
  - **JavaScript**:
    - **script.js**: Handles fetching data from the backend, updating the UI, and processing conversions.

## Technologies Used

- **Backend**: ASP.NET Core 9.0
- **Frontend**: HTML, CSS, JavaScript
- **API**: Central Bank of Russia's exchange rate API
- **Styling**: Custom CSS with a dark theme

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- A web browser (e.g., Chrome, Firefox)
- Git (optional, for cloning the repository)

### Installation and Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/<your-username>/<repository-name>.git
   cd <repository-name>
