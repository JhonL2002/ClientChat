async function handleImageError(imgElement) {
    const blobName = exctractBlobNameFromUrl(imgElement.src);

    //Call C# method in javascript to fetch the SAS URL
    const newUrl = await Dotnet.invokeMethodAsync('ClientChat', 'RegenerateSasUrl', blobName);

    if (newUrl) {
        imgElement.src = newUrl;
    }
}

function exctractBlobNameFromUrl(url) {
    const matches = /\/([^\/?#]+)[^\/]*$/.exec(url);
    return matches && matches[1];
}