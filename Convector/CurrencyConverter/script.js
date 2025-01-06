document.addEventListener("DOMContentLoaded", () => {
    const currencyForm = document.getElementById("currencyForm");
    const fromCurrency = document.getElementById("fromCurrency");
    const toCurrency = document.getElementById("toCurrency");
    const amountInput = document.getElementById("amount");
    const resultInput = document.getElementById("result");
    const switchCurrencies = document.getElementById("switchCurrencies");

    // Load the list of currencies from the API
    fetch('http://localhost:5109/api/currency/currencies')
        .then(response => response.json())
        .then(data => {
            // Add currencies to both dropdown lists
            data.forEach(currency => {
                const optionFrom = document.createElement("option");
                optionFrom.value = currency.code;
                optionFrom.textContent = `${currency.name} (${currency.code})`;
                fromCurrency.appendChild(optionFrom);

                const optionTo = document.createElement("option");
                optionTo.value = currency.code;
                optionTo.textContent = `${currency.name} (${currency.code})`;
                toCurrency.appendChild(optionTo);
            });

            // Set RUB as the default currency
            fromCurrency.value = "RUB";
            toCurrency.value = "USD";
        })
        .catch(() => {
            console.error("Error loading the list of currencies");
        });

    // Handle currency switching
    switchCurrencies.addEventListener("click", () => {
        const temp = fromCurrency.value;
        fromCurrency.value = toCurrency.value;
        toCurrency.value = temp;
        convertCurrency();
    });

    // Handle currency conversion when the form is submitted
    currencyForm.addEventListener("submit", (e) => {
        e.preventDefault();
        convertCurrency();
    });

    // Function to perform currency conversion
    function convertCurrency() {
        const from = fromCurrency.value;
        const to = toCurrency.value;
        const amount = parseFloat(amountInput.value);

        if (isNaN(amount) || amount <= 0) {
            resultInput.value = "Invalid amount";
            return;
        }

        // Example API request (replace the URL with your own API endpoint)
        fetch(`http://localhost:5109/api/currency/convert?fromCurrency=${from}&toCurrency=${to}&amount=${amount}`)
            .then((response) => response.json())
            .then((data) => {
                resultInput.value = data.result.toFixed(2);
            })
            .catch(() => {
                resultInput.value = "Error during conversion";
            });
    }
});
