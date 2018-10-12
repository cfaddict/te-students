document.querySelector('#track').addEventListener("change", function (event) {
    let track = event.target.options[event.target.options.selectedIndex].text;
    location.href = "/students/track/" + track;
});

document.querySelector('#location').addEventListener("change", function (event) {
    console.log("Location change handler");
    let techElevatorLocation = event.target.options[event.target.options.selectedIndex].text;
    console.log(location);
    location.href = "/students/location/" + techElevatorLocation;
});

document.querySelector('#clearFilters').addEventListener("click", function (event) {
    location.href = "/Students/Index";
});