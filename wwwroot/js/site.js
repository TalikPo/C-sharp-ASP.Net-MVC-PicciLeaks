function onSelectedPicture(event) {
    let selectedValue = event.target.id;
    let displayPictureImg = document.getElementById("displayPictureImgId");
    let ajax = new XMLHttpRequest();
    let url = "Gallery/ShowPicture?id=" + selectedValue;
    ajax.open("GET", url, true);
    ajax.onreadystatechange = function () {
        if (ajax.readyState == 4 && ajax.status == 200) {
            let responseJson = JSON.parse(ajax.responseText);
            displayPictureImg.src = responseJson.filePath;
            document.getElementById("downloadPictureAId").setAttribute("href", "/gallery/downloadPicture?filepath=" + responseJson.filePath);
            document.getElementById("fileSizeHId").textContent = "Size in bytes: " + responseJson.fileSize;
            document.getElementById("loadedAtHId").textContent = "Loaded at: " + new Date(responseJson.loadedAt).toLocaleString();
        }
    }
    ajax.send();
}

function patternSearch(event) {
    event.preventDefault();
    let inputPattern = document.getElementById("inputPatternId").value;
    let ajax = new XMLHttpRequest();
    let url = "Gallery/Search?lookingFor=" + inputPattern;
    ajax.open("GET", url, true);
    ajax.onreadystatechange = function () {
        if (ajax.readyState == 4 && ajax.status == 200) {
            let responseJson = JSON.parse(ajax.responseText);
            document.getElementById("displayPicturesDivId").innerHTML = "";
            responseJson.forEach((onePicture) => {

                let newImg = document.createElement("img");
                newImg.src = onePicture.filePath;
                newImg.alt = "Picture";
                newImg.style.width = '100%';

                let newAnchor = document.createElement('a');
                newAnchor.href = "/gallery/downloadPicture?filepath=" + onePicture.filePath;
                newAnchor.appendChild(newImg);

                let newPicDiv = document.createElement("div");
                newPicDiv.className = "image";
                newPicDiv.appendChild(newAnchor);

                document.getElementById("displayPicturesDivId").appendChild(newPicDiv);
            })
        }
    }
    ajax.send();
}

 

