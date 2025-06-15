window.getWindowWidth = () => {
    return window.innerWidth
};

window.checkImagesLoad = function (imageUrls) {
    return Promise.all(imageUrls.map(url => {
        return new Promise(resolve => {
            const img = new Image();
            img.onload = () => resolve({ url, loaded: true });
            img.onerror = () => resolve({ url, loaded: false });
            img.src = url;
        });
    }));
};

window.hideLoader = () => {
    const loader = document.getElementById('initialLoader');
    if (loader) loader.style.display = 'none';
};