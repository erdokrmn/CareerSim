document.addEventListener("DOMContentLoaded", function () {
    // Sayfadaki tüm toggle düğmelerini bul
    document.querySelectorAll("[data-toggle-details]").forEach(function (btn) {
        var target = document.querySelector(btn.getAttribute("data-toggle-details"));
        if (!target) return;

        // Başta gizle
        target.style.display = "none";

        // Butona metin ataması için attr’ları oku
        var showText = btn.getAttribute("data-show-text") || "Detayları Göster";
        var hideText = btn.getAttribute("data-hide-text") || "Detayları Gizle";
        btn.textContent = showText;

        btn.addEventListener("click", function () {
            var isHidden = target.style.display === "none";
            target.style.display = isHidden ? "block" : "none";
            btn.textContent = isHidden ? hideText : showText;
        });
    });
});
