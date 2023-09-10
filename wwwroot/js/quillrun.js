function RunQuill(textId,editor='htmlEditor') {

    var quill = new Quill("#"+editor, {
        modules: {
            toolbar: [

                [{'font': []}],
                ['bold', 'italic', 'underline', 'strike'],
                [{'color': []}, {'background': []}],
                [{'script': 'super'}, {'script': 'sub'}],
                [{'header': '1'}, {'header': '2'}, 'blockquote', 'code-block'],
                [{'list': 'ordered'}, {'list': 'bullet'}, {'indent': '-1'}, {'indent': '+1'}],
                ['direction', {'align': []}],
                ['link', 'image', 'video', 'formula'],
                ['clean']
            ],
        },
        theme: 'snow'
    });
    quill.on('text-change', () => {
        $("#"+textId).val(quill.root.innerHTML);
    });
    document.addEventListener('DOMContentLoaded', function () {
        quill.root.innerHTML = $("#"+textId).val();
    });
}