// Créez un observateur pour surveiller les éléments .blog
const blogs = document.querySelectorAll('.sub-blog');

const observer = new IntersectionObserver(entries => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.classList.add('animate');  // Ajoute la classe animate lorsque l'élément est visible
        }
    });
}, { threshold: 0.5 });  // L'élément doit être à 50% visible pour déclencher l'animation

blogs.forEach(subblog => {
    observer.observe(subblog);
});

document.getElementById('searchForm').addEventListener('submit', function(event) {
    event.preventDefault(); // Empêche l'envoi du formulaire et la redirection
    var query = document.getElementById('searchInput').value.toLowerCase();
    alert("Recherche pour : " + query); // Remplace ceci par une logique de recherche
});