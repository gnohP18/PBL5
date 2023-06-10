$(function () {
    $("#adminMode").on("click", function (e) {
        window.location.href="/Account/Login"
    })

    let weekColSun = $('.weekColSun')
    let weekColMon = $('.weekColMon')
    let weekColTue = $('.weekColTue')
    let weekColWed = $('.weekColWed')
    let weekColThu = $('.weekColThu')
    let weekColFri = $('.weekColFri')
    let weekColSat = $('.weekColSat')

    const now = new Date(); // Lấy ngày hiện tại
    const firstDayOfPresentMonth = new Date(now.getFullYear(), now.getMonth(), 1) // Ngày đầu tiên trong tháng hiện tại

    //Trả về ngày đầu tiên trong tháng trúng thứ mấy (0 - Sun, 1 - Mon, 2 -Tue, 3 -Wed, 4 -Thu, 5 -Fri, 6 -Sat Sat)
    const indexBegin = firstDayOfPresentMonth.getDay()

    //Ngày đầu tiên của tháng kế tiếp
    const firstDayOfNextMonth = new Date(now.getFullYear(), now.getMonth() + 1, 1)

    //Ngày cuối cùng của tháng hiện tại
    const lastDayOfPresentMonth = new Date(firstDayOfNextMonth - 1)

    //Trả về số ngày của tháng hiện tại
    const numberOfDayPresentMonth = lastDayOfPresentMonth.getDate()
    
    // for (let i = 0; i < 7; i++) {
    //     for(let j = 0; j < 5; j++) {

    //     }
    // }

    for (let i = 0; i < 35; i++) {
        if (i < indexBegin || i > numberOfDayPresentMonth){
            if ( i % 7 == 0) { weekColSun.append('<abp-row class="dates"></abp-row>')}
            if ( i % 7 == 1) { weekColMon.append('<abp-row class="dates"></abp-row>')}
            if ( i % 7 == 2) { weekColTue.append('<abp-row class="dates"></abp-row>')}
            if ( i % 7 == 3) { weekColWed.append('<abp-row class="dates"></abp-row>')}
            if ( i % 7 == 4) { weekColThu.append('<abp-row class="dates"></abp-row>')}
            if ( i % 7 == 5) { weekColFri.append('<abp-row class="dates"></abp-row>')}
            if ( i % 7 == 6) { weekColSat.append('<abp-row class="dates"></abp-row>')}
        }
        else{
            if ( i % 7 == 0) { weekColSun.append(`<abp-row class="dates"><h4>${i}<h4></abp-row>`)}
            if ( i % 7 == 1) { weekColMon.append(`<abp-row class="dates"><h4>${i}<h4></abp-row>`)}
            if ( i % 7 == 2) { weekColTue.append(`<abp-row class="dates"><h4>${i}<h4></abp-row>`)}
            if ( i % 7 == 3) { weekColWed.append(`<abp-row class="dates"><h4>${i}<h4></abp-row>`)}
            if ( i % 7 == 4) { weekColThu.append(`<abp-row class="dates"><h4>${i}<h4></abp-row>`)}
            if ( i % 7 == 5) { weekColFri.append(`<abp-row class="dates"><h4>${i}<h4></abp-row>`)}
            if ( i % 7 == 6) { weekColSat.append(`<abp-row class="dates"><h4>${i}<h4></abp-row>`)}
        }
    }
});
