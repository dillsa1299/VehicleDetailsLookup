window.getWindowWidth = () => {
    return window.innerWidth
};

window.checkImagesLoad = function (imageUrls) {
    return Promise.all(imageUrls.map(url => {
        return new Promise(resolve => {
            const img = new Image();
            img.onload = () => resolve(url); // Return the URL string on success
            img.onerror = () => resolve(null); // Return null on error
            img.src = url;
        });
    })).then(results => results.filter(url => url)); // Filter out nulls
};

window.hideLoader = () => {
    const loader = document.getElementById('initialLoader');
    if (loader) loader.style.display = 'none';
};