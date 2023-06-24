// Функция для отправки запроса к API
window.myFunctions = {


    fetchAPI: function (url, data, method = 'POST', autchToken='') {
        const headers = {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:109.0) Gecko/20100101 Firefox/111.0",
            "Accept": "*/*",
            "Accept-Language": "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3",
            "Content-Type": "application/x-www-form-urlencoded; charset=UTF-8"
        };

        return fetch(url, {
            "credentials": "omit",
            "referrer": "https://a-n-h.space/raspisanie",
            headers,
            'method': method,
            'body': data
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                }
                // обработка ошибок
            });
    },

    // Функция для получения JSON из API
    getJson: function (method, body, url) {
        return window.myFunctions.fetchAPI(url, body, method)
            .then(data => {
                // здесь ты можешь использовать data как объект JSON
                console.log(data);
                return data;
            })
            .catch(error => {
                // здесь ты можешь обработать ошибки
                console.error(error);
            });
    }
}