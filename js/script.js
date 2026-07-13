let translations = {};
let currentLang = 'en';

function loadHTML(id, file) {
  const el = document.getElementById(id);
  if (!el) return Promise.resolve();

  return fetch(file)
    .then(res => res.text())
    .then(data => {
      el.innerHTML = data;
    })
    .catch(err => console.error("Error cargando " + file, err));
}

function recalculateOpenAccordions() {
  document.querySelectorAll('.accordion-item.active').forEach(item => {
    const content = item.querySelector('.accordion-content');
    if (content) {
      content.style.maxHeight = content.scrollHeight + "px";
    }
  });
}

function getBrowserLanguage() {
  const browserLang = (navigator.language || navigator.userLanguage).toLowerCase();

  const shortLang = browserLang.split('-')[0];

  const supportedLanguages = ['es', 'en'];

  return supportedLanguages.includes(shortLang) ? shortLang : 'en';
}

async function loadLanguage(lang) {
  try {
    const res = await fetch(`/lenguages/${lang}.json`);
    translations = await res.json();
  } catch (err) {
    console.error("Error cargando idioma:", err);
  }
}

function updateTexts() {
  const currentYear = new Date().getFullYear();

  document.querySelectorAll('[data-i18n]').forEach(el => {
    const key = el.getAttribute('data-i18n');
    if (translations[key]) {
      let text = translations[key];
      if (text.includes('{{year}}')) {
        text = text.replace('{{year}}', currentYear);
      }
      el.innerHTML = text;
    }
  });

  document.querySelectorAll('[data-i18n-img]').forEach(img => {
    img.src = `/imgs/moonchild/moonchild_mail_${currentLang}.png`;
  });
}

async function loadCode(id, file) {
  const el = document.getElementById(id);
  if (!el) return;

  try {
    const res = await fetch(file);
    const text = await res.text();

    el.textContent = text;
    Prism.highlightElement(el);

    recalculateOpenAccordions();
  } catch (err) {
    console.error("Error cargando código:", err);
  }
}

async function init() {
  const isIndex = document.getElementById("projects") !== null;

  const savedLang = localStorage.getItem('user-language');

  if (savedLang) {
    currentLang = savedLang;
  } else {
    currentLang = getBrowserLanguage();
    localStorage.setItem('user-language', currentLang);
  }

  await Promise.all([
    loadHTML("header", "/html/header.html"),
    loadLanguage(currentLang)
  ]);

  const navBtn = document.querySelector('.nav-responsive');
  const navUl = document.querySelector('nav ul');

  if (navBtn && navUl) {
    navBtn.addEventListener('click', () => {
      navUl.classList.toggle('show');
    });
  }

  const navLinks = document.querySelectorAll('header a[href^="#"]');

  navLinks.forEach(link => {
    link.addEventListener("click", function (e) {
      e.preventDefault();

      const target = this.getAttribute("href");
      const currentPage = window.location.pathname;

      if (currentPage.includes("index.html") || currentPage === "/") {
        window.location.hash = target;
      } else {
        window.location.href = "/index.html" + target;
      }
    });
  });

  if (isIndex) {
    await Promise.all([
      loadHTML("aboutme", "/html/aboutme.html"),
      loadHTML("projects", "/html/projects.html"),
      loadHTML("contact", "/html/contact.html"),
      loadHTML("footer", "/html/footer.html")
    ]);
  } else {
    await loadHTML("footer", "/html/footer.html");
  }

  updateTexts();

  const langBtn = document.getElementById('langBtn');
  const langMenu = document.getElementById('langMenu');
  const currentFlag = document.getElementById('currentFlag');
  const currentLangText = document.getElementById('currentLangText');

  if (langBtn && langMenu) {
    currentFlag.src = `https://flagcdn.com/w20/${currentLang === 'en' ? 'gb' : 'es'}.png`;
    currentLangText.textContent = currentLang === 'en' ? 'En' : 'Es';

    langBtn.addEventListener('click', (e) => {
      e.stopPropagation();
      const isDisplayed = langMenu.style.display === 'block';
      langMenu.style.display = isDisplayed ? 'none' : 'block';
    });

    langMenu.querySelectorAll('li').forEach(item => {
      item.addEventListener('click', async () => {
        currentLang = item.getAttribute('data-value');
        localStorage.setItem('user-language', currentLang);

        currentFlag.src = `https://flagcdn.com/w20/${currentLang === 'en' ? 'gb' : 'es'}.png`;
        currentLangText.textContent = currentLang === 'en' ? 'En' : 'Es';

        await loadLanguage(currentLang);
        updateTexts();

        recalculateOpenAccordions();

        langMenu.style.display = 'none';
      });
      
      item.addEventListener('mouseenter', () => item.style.background = 'var(--color-gray)');
      item.addEventListener('mouseleave', () => item.style.background = 'transparent');
    });

    document.addEventListener('click', () => {
      langMenu.style.display = 'none';
    });
  }

  if (window.location.hash) {
    const el = document.querySelector(window.location.hash);
    if (el) {
      el.scrollIntoView({ behavior: "smooth" });
    }
  }

  document.querySelectorAll('.accordion-header').forEach(header => {
    header.addEventListener('click', () => {
      const item = header.parentElement;
      const content = item.querySelector('.accordion-content');

      item.classList.toggle('active');

      if (item.classList.contains('active')) {
        content.style.maxHeight = content.scrollHeight + "px";
      } else {
        content.style.maxHeight = "0";
      }
    });
  });

  loadCode("code-base-state", "/code/dusty/base_state.h");
  loadCode("code-base-state-character", "/code/dusty/base_state_character.h");
  loadCode("code-action-filter-h", "/code/dusty/action_filter.h");
  loadCode("code-action-filter-cpp", "/code/dusty/action_filter.cpp");
  loadCode("code-states-data-asset", "/code/dusty/states_data_asset.h");
  loadCode("code-episode-scriptable-object", "/code/bluey/EpisodeScriptableObject.cs");
  loadCode("code-checkpoint-base", "/code/bluey/CheckpointBase.cs");
  loadCode("code-cinematic-checkpoint", "/code/bluey/CinematicCheckpoint.cs");
  loadCode("code-mission-checkpoint", "/code/bluey/MissionCheckpoint.cs");
}

window.addEventListener('DOMContentLoaded', init);