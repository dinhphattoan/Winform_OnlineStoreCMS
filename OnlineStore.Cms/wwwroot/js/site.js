// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



document.addEventListener('DOMContentLoaded', function () {
    const images = document.querySelectorAll('img');

    images.forEach(img => {
        img.addEventListener('error', function handleError() {
            const defaultImage = 'https://phutungnhapkhauchinhhang.com/wp-content/uploads/2020/06/default-thumbnail.jpg';

            img.src = defaultImage;
            img.alt = 'default';
        });
    });

    const liveToast = document.getElementById('liveToast');
    const toast = new bootstrap.Toast(liveToast);

    toast.show();
});