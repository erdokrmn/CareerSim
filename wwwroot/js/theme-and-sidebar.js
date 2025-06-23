document.addEventListener('DOMContentLoaded', function () {
    const body = document.body;
    const themeBtn = document.getElementById('themeToggle');
    const sidebarBtn = document.getElementById('sidebarToggle');
    const sidebar = document.getElementById('sidebar');

    // Tema yükleme
    const saved = localStorage.getItem('theme') || 'light';
    body.classList.toggle('dark', saved === 'dark');

    // Tema toggle
    themeBtn.addEventListener('click', () => {
        const isDark = body.classList.toggle('dark');
        localStorage.setItem('theme', isDark ? 'dark' : 'light');
    });

    // Sidebar toggle
    sidebarBtn.addEventListener('click', () => {
        sidebar.classList.toggle('collapsed');
    });
});
