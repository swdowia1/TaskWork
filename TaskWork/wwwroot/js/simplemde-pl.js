/*
 * SimpleMDE – automatyczna polska wersja interfejsu
 * Autor: ChatGPT (GPT-5)
 * Wersja: 1.1
 * Umieść w: wwwroot/js/simplemde-pl.js
 * Działa z SimpleMDE v1.11+
 */

(function () {
    // Sprawdź, czy SimpleMDE jest dostępny
    function applyPolish() {
        if (typeof SimpleMDE === "undefined") {
            console.error("❌ SimpleMDE nie został jeszcze załadowany – wczytaj simplemde.min.js przed simplemde-pl.js");
            return;
        }

        // 🔧 Ustawienia domyślne (PL)
        SimpleMDE.defaults = Object.assign(SimpleMDE.defaults || {}, {
            placeholder: "Wpisz tutaj swój tekst w Markdown...",
            spellChecker: false,
            status: [
                {
                    className: "words",
                    defaultValue: function (el) {
                        el.innerHTML = "0 słów";
                    },
                    onUpdate: function (el) {
                        const cm = el.closest(".CodeMirror").CodeMirror;
                        const words = cm.getValue().split(/\s+/).filter(Boolean).length;
                        el.innerHTML = words + " słów";
                    },
                },
                { className: "lines", defaultValue: "Linie" },
                { className: "mode", defaultValue: "Tryb edycji" },
            ],
            toolbar: [
                { name: "bold", action: SimpleMDE.toggleBold, className: "fa fa-bold", title: "Pogrubienie (Ctrl+B)" },
                { name: "italic", action: SimpleMDE.toggleItalic, className: "fa fa-italic", title: "Kursywa (Ctrl+I)" },
                { name: "heading", action: SimpleMDE.toggleHeadingSmaller, className: "fa fa-header", title: "Nagłówek" },
                "|",
                { name: "quote", action: SimpleMDE.toggleBlockquote, className: "fa fa-quote-left", title: "Cytat" },
                { name: "unordered-list", action: SimpleMDE.toggleUnorderedList, className: "fa fa-list-ul", title: "Lista punktowana" },
                { name: "ordered-list", action: SimpleMDE.toggleOrderedList, className: "fa fa-list-ol", title: "Lista numerowana" },
                "|",
                { name: "link", action: SimpleMDE.drawLink, className: "fa fa-link", title: "Wstaw link" },
                { name: "image", action: SimpleMDE.drawImage, className: "fa fa-picture-o", title: "Wstaw obraz" },
                { name: "table", action: SimpleMDE.drawTable, className: "fa fa-table", title: "Wstaw tabelę" },
                "|",
                { name: "preview", action: SimpleMDE.togglePreview, className: "fa fa-eye", title: "Podgląd" },
                { name: "side-by-side", action: SimpleMDE.toggleSideBySide, className: "fa fa-columns", title: "Podgląd obok siebie" },
                { name: "fullscreen", action: SimpleMDE.toggleFullScreen, className: "fa fa-arrows-alt", title: "Pełny ekran" },
                "|",
                { name: "guide", action: "https://simplemde.com/markdown-guide", className: "fa fa-question-circle", title: "Pomoc Markdown" },
            ],
        });

        console.info("✅ SimpleMDE: interfejs ustawiony na język polski");

        // 🔄 Automatyczne uruchamianie edytora
        document.querySelectorAll("textarea[data-simplemde]").forEach(el => {
            if (!el.dataset.simplemdeInitialized) {
                new SimpleMDE({ element: el });
                el.dataset.simplemdeInitialized = "true";
            }
        });
    }

    // Odpal po załadowaniu DOM
    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", applyPolish);
    } else {
        applyPolish();
    }
})();
