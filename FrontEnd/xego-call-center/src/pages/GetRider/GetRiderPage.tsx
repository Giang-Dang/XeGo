import { useLocation } from "react-router-dom";

export function GetRiderPage() : React.ReactElement {
  const location = useLocation();

  const pageState = location.state;
  
  console.log(location.state);
  return(
    <>
      Get Ride Page
    </>
  ); 
}