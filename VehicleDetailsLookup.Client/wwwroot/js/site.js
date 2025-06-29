window.getWindowWidth = () => {
    return window.innerWidth
};

window.preloadImages = async function (imageUrls) {
    const results = await Promise.all(imageUrls.map(url => {
        return new Promise(resolve => {
            const img = new Image();
            img.onload = () => resolve(url); // Return the URL string on success
            img.onerror = () => resolve(null); // Return null on error
            img.src = url;
        });
    }));
    return results.filter(url => url); // Filter out nulls
};

window.hideLoader = () => {
    const loader = document.getElementById('initialLoader');
    if (loader) loader.style.display = 'none';
};