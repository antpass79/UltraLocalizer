function SaveFileAs(filename, fileContent) {
    var blob = new Blob([fileContent], { type: "application/octet-stream" });

    var link = document.createElement('a');
    link.download = filename;
    var url = window.URL.createObjectURL(blob);
    link.href = url;
    document.body.appendChild(link);
    link.click();
    window.URL.revokeObjectURL(url);
    document.body.removeChild(link);
}