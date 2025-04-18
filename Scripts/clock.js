var ringer = {
    rings: {
        'Heures': {
            s: 3600000, // millisecondes par heure
            max: 24
        },
        'Minutes': {
            s: 60000, // millisecondes par minute
            max: 60
        },
        'Secondes': {
            s: 1000,
            max: 60
        }
    },
    r_count: 3, // nombre de cercles
    r_spacing: 10, // px
    r_size: 100, // px
    r_thickness: 2, // px
    update_interval: 11, // ms

    init: function () {
        $r = ringer;
        $r.cvs = document.createElement('canvas');
        $r.cvs.id = "clock";

        $r.size = {
            w: ($r.r_size + $r.r_thickness) * $r.r_count + ($r.r_spacing * ($r.r_count - 1)),
            h: ($r.r_size + $r.r_thickness)
        };

        $r.cvs.setAttribute('width', $r.size.w);
        $r.cvs.setAttribute('height', $r.size.h);
        $r.ctx = $r.cvs.getContext('2d');
        $(document.querySelector('#topInfos')).append($r.cvs);
        $r.cvs = $($r.cvs);
        $r.ctx.textAlign = 'center';
        $r.actual_size = $r.r_size + $r.r_thickness;
        $r.cvs.css({ width: $r.size.w + "px", height: $r.size.h + "px" });
        $r.go();
    },

    ctx: null,

    go: function () {
        var idx = 0;
        var dt = new Date();
        var msecs = (dt.getSeconds() + (60 * dt.getMinutes()) + (60 * 60 * dt.getHours())) * 1000;
        $r.time = msecs;

        for (var r_key in $r.rings) {
            $r.unit(idx++, r_key, $r.rings[r_key]);
        }

        setTimeout($r.go, $r.update_interval);
    },

    unit: function (idx, label, ring) {
        var x, y, value, ring_secs = ring.s;
        value = parseFloat($r.time / ring_secs);
        $r.time -= Math.round(parseInt(value)) * ring_secs;
        value = Math.abs(value);

        x = ($r.r_size * .5 + $r.r_thickness * .5);
        x += +(idx * ($r.r_size + $r.r_spacing + $r.r_thickness));
        y = $r.r_size * .5;
        y += $r.r_thickness * .5;

        let startAngle = -Math.PI / 2;
        let endAngle = startAngle + (value / ring.max) * 2 * Math.PI;

        $r.ctx.save();
        $r.ctx.translate(x, y);
        $r.ctx.clearRect($r.actual_size * -0.5, $r.actual_size * -0.5, $r.actual_size, $r.actual_size);

        // Cercle de fond
        $r.ctx.strokeStyle = "rgba(128,128,128,0.2)";
        $r.ctx.beginPath();
        $r.ctx.arc(0, 0, $r.r_size / 2, 0, 2 * Math.PI);
        $r.ctx.lineWidth = $r.r_thickness;
        $r.ctx.stroke();

        // Cercle de progression
        $r.ctx.strokeStyle = "rgba(253, 128, 1, 0.9)";
        $r.ctx.beginPath();
        $r.ctx.arc(0, 0, $r.r_size / 2, startAngle, endAngle); // début à 12h
        $r.ctx.lineWidth = $r.r_thickness;
        $r.ctx.stroke();

        // Texte
        $r.ctx.fillStyle = "black";
        $r.ctx.font = '12px Helvetica';
        $r.ctx.fillText(label, 0, 23);
        $r.ctx.font = 'bold 40px Helvetica';
        $r.ctx.fillText(Math.floor(value), 0, 10);

        $r.ctx.restore();
    }
};

ringer.init();
