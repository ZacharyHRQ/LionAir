function myFunction() {
    var input, filter, table, tr, td, i, txtValue, dropdown;

    input = document.getElementById("search");

    filter = input.value.toUpperCase();

    table = document.getElementById("myTable");

    tr = table.getElementsByTagName("tr");

    dropdown = $('#options').val();

    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[dropdown];
        console.log(td)
        console.log(dropdown)

        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}

document.getElementById('button').addEventListener("click", function () {
    document.querySelector('.bg-modal').style.display = "flex";
});

document.querySelector('.close').addEventListener("click", function () {
    document.querySelector('.bg-modal').style.display = "none";
});