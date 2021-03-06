#load "models.fs"
open System.Windows.Forms
open System.Drawing
open Models
let view (sz: Size) (shape: (Size -> Shape)) : (unit -> unit) =
    let _paintFunc (e:PaintEventArgs) = 
        let _shape : Shape = 
            Size (e.ClipRectangle.Width, e.ClipRectangle.Height) |> shape
        
        let rec _drawShape (g:Graphics) (s:Shape) : unit =
            match s with 
            | Circle (pen, center, radius) ->
                let p1 = Point (center.X - radius, center.Y - radius)
                let p2 = Point (center.X + radius, center.Y + radius)
                // let rect = Rectangle (p1.X, p1.Y, p2.X, p2.Y)
                let rect = Rectangle (p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y)

                g.DrawEllipse (pen, rect)
            | FilledCircle (brush, center, radius) -> 
                let p1 = Point (center.X - radius, center.Y - radius)
                let p2 = Point (center.X + radius, center.Y + radius)
                let rect = Rectangle (p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y)
                g.FillEllipse (brush, rect)

            | Polygon (pen, pointList) -> 
                printfn "huh"
                g.DrawLines(pen, pointList)
            | Line (pen, p1, p2) -> 
                g.DrawLine (pen, p1, p2)
            | Mix (s1, s2) -> 
                _drawShape g s1;
                _drawShape g s2;
        
        _drawShape e.Graphics _shape 
    

    let tick = new Timer ()
    tick.Interval <- 5000
    tick.Enabled <- true

    let win = new Form ()
    win.Name <- "Clock" // Workaround for linux WM
    win.Text <- "Clock"
    win.BackColor <- Color.White
    win.ClientSize <- sz
    win.Paint.Add _paintFunc

    tick.Tick.Add (fun e -> win.Refresh () )


    fun () -> Application.Run win 
    
let run = view (Size (500, 500) ) Models.watchFace
run () 

