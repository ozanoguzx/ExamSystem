function Bilgilendirme(text, tip) {
    alertify.log('<strong>' + text + '</strong>', tip);
}

function Info(text, tip) {
    alertify.set({ delay: 10000 });
    alertify.log('<strong>' + text + '</strong>', tip);
}