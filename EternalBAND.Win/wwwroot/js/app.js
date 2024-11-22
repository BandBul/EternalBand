// image upload - dropzone

Dropzone.autoDiscover = false; // Disable auto-discovery to prevent conflicts

let deletedFiles = []; // To store deleted file identifiers
let uploadedFiles = []; // Stores files added in this session

function windowScroll() { var t = document.getElementById("navbar"); 50 <= document.body.scrollTop || 50 <= document.documentElement.scrollTop ? t.classList.add("nav-sticky") : t.classList.remove("nav-sticky") } window.addEventListener("scroll", function (t) { t.preventDefault(), windowScroll() }); var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]')), tooltipList = tooltipTriggerList.map(function (t) { return new bootstrap.Tooltip(t) });

function readURL(input) {
    if (input.files && input.files[0]) {

        var reader = new FileReader();

        reader.onload = function (e) {
            $('.image-upload-wrap').hide();

            $('.file-upload-image').attr('src', e.target.result);
            $('.file-upload-content').show();

            $('.image-title').html(input.files[0].name + " <br><br>Fotoğrafı Değiştir");
        };

        reader.readAsDataURL(input.files[0]);

    } else {
        removeUpload();
    }
}
function removeUpload() {
    $('.file-upload-input').replaceWith($('.file-upload-input').clone());
    $('.file-upload-content').hide();
    $('.image-upload-wrap').show();
}
function LoadShowPhoto() {
    if ($(".file-upload-image").attr('src').length > 0) {
        $('.image-upload-wrap').hide();
        $('.file-upload-content').show();
    }
}

function initDropzone(preloadedFiles = []) {
    // Initialize Dropzone
    const dropzone = new Dropzone("#image-upload", {
        url: "#", // Replace with your upload endpoint
        autoProcessQueue: false, // Prevent auto-upload
        maxFilesize: 2, // MB
        maxFiles: 5,
        addRemoveLinks: true,
        dictDefaultMessage: "Drop files here or click the button to upload",
        init: function () {
            const dz = this;

            // Add preloaded files
            preloadedFiles.forEach(function (file) {
                const mockFile = { name: `Image ${file.id}`, size: 12345, serverId: file.id };
                dz.emit("addedfile", mockFile);
                dz.emit("thumbnail", mockFile, file.src);
                dz.emit("complete", mockFile);
                dz.files.push(mockFile);
            });

            // Handle file addition
            dz.on("addedfile", function (file) {
                if (!file.isMock) {
                    if (dz.files.length > this.options.maxFiles) {
                        this.removeFile(file);
                        alert("You can only upload up to 5 files.");
                    }
                    else {
                        uploadedFiles.push(file);
                    }
                }
            });

            // Handle file removal
            dz.on("removedfile", function (file) {
                if (file.serverId) {
                    deletedFiles.push(file.serverId);
                }
            });

            dz.on("maxfilesexceeded", function (file) {
                this.removeFile(file); // Remove the extra file
                alert("You can only upload up to 5 files.");
            });
        }
    });

}
document.addEventListener("DOMContentLoaded", function(){
// make it as accordion for smaller screens
    if (window.innerWidth < 992) {

        // close all inner dropdowns when parent is closed
        document.querySelectorAll('.navbar .dropdown').forEach(function(everydropdown){
            everydropdown.addEventListener('hidden.bs.dropdown', function () {
                // after dropdown is hidden, then find all submenus
                this.querySelectorAll('.submenu').forEach(function(everysubmenu){
                    // hide every submenu as well
                    everysubmenu.style.display = 'none';
                });
            })
        });

        document.querySelectorAll('.dropdown-menu a').forEach(function(element){
            element.addEventListener('click', function (e) {
                let nextEl = this.nextElementSibling;
                if(nextEl && nextEl.classList.contains('submenu')) {
                    // prevent opening link if link needs to open dropdown
                    e.preventDefault();
                    if(nextEl.style.display == 'block'){
                        nextEl.style.display = 'none';
                    } else {
                        nextEl.style.display = 'block';
                    }

                }
            });
        })
    }
// end if innerWidth
});
// DOMContentLoaded  end

// image upload for post edit
const formEdit = document.getElementById('musicFormEdit');
if (formEdit) {
    document.getElementById('musicFormEdit').addEventListener('submit', function (e) {
        e.preventDefault();

        const formData = new FormData(this);

        // Append uploaded files
        uploadedFiles.forEach(file => {
            formData.append('uploadedFiles', file);
        });

        // Add deleted files as a separate parameter
        deletedFiles.forEach(index => {
            formData.append('deletedFilesIndex', index);
        });

        fetch('/ilan-duzenle', {
            method: 'POST',
            body: formData,
        })
            .then(response => {
                if (response.ok) {
                    console.info('Changes saved successfully.');
                    window.location.href = "/ilanlarim";

                } else {
                    console.info('Error saving changes.');
                }
            })
            .catch(error => console.error('Error:', error));
    });
}



// image upload for post create
const formCreate = document.getElementById('musicFormCreate');
if (formCreate)
{
    document.getElementById('musicFormCreate').addEventListener('submit', function (e) {
        e.preventDefault();

        const formData = new FormData(this);

        // Append uploaded files
        uploadedFiles.forEach(file => {
            formData.append('uploadedFiles', file);
        });

        // Add deleted files as a separate parameter
        deletedFiles.forEach(index => {
            formData.append('deletedFilesIndex', index);
        });

        fetch('/PostCreate', {
            method: 'POST',
            body: formData,
        })
            .then(response => {
                if (response.ok) {
                    console.info('Changes saved successfully.');
                    window.location.href = "/ilanlarim";

                } else {
                    console.info('Error saving changes.');
                }
            })
            .catch(error => console.error('Error:', error));
    });
}