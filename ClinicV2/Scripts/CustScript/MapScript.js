
function test() {
    for (var i = 0; i < markers.length; i++) {
        alert('hi');
    }
}
function Marker(addLookup, idName) {
    alert("step1");
    if (document.getElementById(idName).checked == true) {
        alert("step2");
        var temp = geocode(addLookup);
        alert(temp);
        alert("stepExist");
        AddMarker(temp);
    }
    else {
  
        RemoveMarker(geocode(addLookup));
    }
}


function geocode(addLookup,idName) {
    var location;
    var coord;
    var lat, lng;
    //alert("step3");
    if (addLookup == 'Empty') {
        
         var zip = document.getElementById('newPatient_Zip').value;
        var street = document.getElementById('newPatient_Street').value;
        var city = document.getElementById('newPatient_State').value;;
        var state = document.getElementById('newPatient_Street').value;;
        if (street == "" || city =="" || state == "") {
            location = zip;
        }
        else if (street != "" && city != "" && state != "" && zip != "") {
            location = street + " " + city + " " + state + " " + zip; 
        }
    }
    else {

        location = addLookup;
    }

    if (location != null) {
        //alert("step4");
        axios.get('https://maps.googleapis.com/maps/api/geocode/json', {
            params: {
                address: location,
                key: 'AIzaSyDmfeWr8pGc9LSDA5BCgCfvj0i3pqoE_cA'
            }
        })
            .then(function (response) {
                //alert("step5");
                 lat = response.data.results[0].geometry.location.lat;
                lng = response.data.results[0].geometry.location.lng;
                //alert(typeof lat);
                if (addLookup == 'Empty') {
                    AddMarker({ coord: { lat: lat, lng: lng } });
                }
                else {
                    if (document.getElementById(idName).checked == true) {

                        AddMarker({ coord: { lat: lat, lng: lng } });
                    
                    }
                    else {
            
                        RemoveMarker({ coord: { lat: lat, lng: lng } });
                    }
                }
           
             
                //coordi = {
                //    coordi: { lat: lat, lng: lng }
                //};
                //console.log(coordi);
               
               console.log(response);
                console.log(response.data.results[0].formatted_address);
                return coord;
            })
            .catch(function (error) {
                console.log(error);
            }
            );
    }

  
}


function distance(lat1, lon1, lat2, lon2) {

    if ((lat1 == lat2) && (lon1 == lon2)) {
        return 0;
    }
    else {
        var radlat1 = Math.PI * lat1 / 180;
        var radlat2 = Math.PI * lat2 / 180;
        var radlon1 = Math.PI * lon1 / 180;
        var radlon2 = Math.PI * lon2 / 180;

        var lon = radlon2 - radlon1;
        var lat = radlon2 - radlon1;
        
        var a = Math.pow(Math.sin(lat / 2), 2)
            + Math.cos(radlat1)
            * Math.cos(radlat2)
            * Math.pow(Math.sin(lon / 2), 2);

        var c = 2 * Math.asin(Math.sqrt(a));

        //radius of earth in miles, use 6371 for kilometers
        var r = 3956;


        return (c * r);
    }
}
  