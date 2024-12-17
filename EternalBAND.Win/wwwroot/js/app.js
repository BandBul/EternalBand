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
        maxFilesize: 7 , // MB
        maxFiles: 5,
        addRemoveLinks: true,
        dictDefaultMessage: "Dosyaları buraya sürükleyin veya yüklemek için tıklayın (Maks. 7 MB).",
        init: function () {
            const dz = this;
            var maxSizeByte = dz.options.maxFilesize * 1024 * 1024;
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
                    var removeFileFlag = false;
                    if (dz.files.length > dz.options.maxFiles)
                    {
                        removeFileFlag = true;
                        alert("En fazla 5 dosya yükleyebilirsiniz.");
                    }
                    if (file.size > maxSizeByte)
                    {
                        removeFileFlag = true;
                        alert("Yüklemek istediğiniz dosya boyutu 7 MB'ı aşıyor. Lütfen daha küçük bir dosya seçin.");
                    }
                    else
                    {
                        uploadedFiles.push(file);
                    }

                    if (removeFileFlag === true) {
                        dz.removeFile(file);
                    }
                }
            });

            // Handle file removal
            dz.on("removedfile", function (file) {
                if (file.serverId) {
                    deletedFiles.push(file.serverId);
                }
            });

            //dz.on("maxfilesexceeded", function (file) {
            //    dz.removeFile(file); // Remove the extra file
            //    alert("En fazla 5 dosya yükleyebilirsiniz.");
            //});

            //dz.on("error", function (file, message) {
            //    if (file.size > dz.options.maxFilesize * 1024 * 1024) {
            //        dz.removeFile(file); // Remove the file exceeding the size
            //        alert("Yüklemek istediğiniz dosya boyutu 7 MB'ı aşıyor. Lütfen daha küçük bir dosya seçin.");
            //    } else {
            //        alert(message); // Handle other errors
            //    }
            //});
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

        fetch('/ilanDuzenleme', {
            method: 'POST',
            body: formData,
        })
            .then(response => {
                if (response.ok) {
                    console.info('Changes saved successfully.');
                    window.location.href = "/ilanlarim";

                } else {
                    console.info('Error saving changes.');
                    // Create a hidden form to simulate a POST redirect
                    const form = document.createElement('form');
                    form.method = 'POST';
                    form.action = 'ilanDuzenleme'; // Server endpoint

                    // Add form data fields to the hidden form
                    for (const [key, value] of formData.entries()) {
                        const input = document.createElement('input');
                        input.type = 'hidden';
                        input.name = key;
                        input.value = value;
                        form.appendChild(input);
                    }

                    document.body.appendChild(form); // Append to body
                    form.submit(); // Submit the form 
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

        fetch('/ilanOlusturma', {
            method: 'POST',
            body: formData,
        })
            .then(response => {
                if (response.ok) {
                    console.info('Changes saved successfully.');
                    window.location.href = "/ilanlarim";

                } else {
                    console.info('Error saving changes.');
                    // Create a hidden form to simulate a POST redirect
                    const form = document.createElement('form');
                    form.method = 'POST';
                    form.action = '/ilanOlusturma'; // Server endpoint

                    // Add form data fields to the hidden form
                    for (const [key, value] of formData.entries()) {
                        const input = document.createElement('input');
                        input.type = 'hidden';
                        input.name = key;
                        input.value = value;
                        form.appendChild(input);
                    }

                    document.body.appendChild(form); // Append to body
                    form.submit(); // Submit the form 
                }
            })
            .catch(error => console.error('Error:', error));
    });
}