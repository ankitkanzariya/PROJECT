-- phpMyAdmin SQL Dump
-- version 4.0.4
-- http://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: Aug 07, 2018 at 04:32 AM
-- Server version: 5.6.12-log
-- PHP Version: 5.4.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Database: `bbdms`
--
CREATE DATABASE IF NOT EXISTS `bbdms` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `bbdms`;

-- --------------------------------------------------------

--
-- Table structure for table `admin`
--

CREATE TABLE IF NOT EXISTS `admin` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(100) NOT NULL,
  `Password` varchar(100) NOT NULL,
  `updationDate` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `admin`
--

INSERT INTO `admin` (`id`, `UserName`, `Password`, `updationDate`) VALUES
(1, 'admin', '12345', '2018-08-03 04:29:57');

-- --------------------------------------------------------

--
-- Table structure for table `tblblooddonars`
--

CREATE TABLE IF NOT EXISTS `tblblooddonars` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `FullName` varchar(100) DEFAULT NULL,
  `MobileNumber` char(11) DEFAULT NULL,
  `EmailId` varchar(100) DEFAULT NULL,
  `Gender` varchar(20) DEFAULT NULL,
  `Age` int(11) DEFAULT NULL,
  `BloodGroup` varchar(20) DEFAULT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `Message` mediumtext,
  `PostingDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `status` int(1) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=19 ;

--
-- Dumping data for table `tblblooddonars`
--

INSERT INTO `tblblooddonars` (`id`, `FullName`, `MobileNumber`, `EmailId`, `Gender`, `Age`, `BloodGroup`, `Address`, `Message`, `PostingDate`, `status`) VALUES
(7, 'Dhranga Manoj D.', '8238989650', 'dhrangamanoj@gmail.com', 'Male', 22, 'A+', 'Ramapir Chowkdi,Gandhigram,Rajkot', 'I  blood donation rates in decline all over the developed world, Sweden’s blood service is enlisting new technology to help push back against shortages. ', '2018-08-07 03:40:04', 1),
(8, 'vala gaurav maganbhai', '9624857559', 'valagaurav111@gmail.com', 'Male', 19, 'AB+', '"khodiyar krupa"\r\n''150 ft. ring road\r\nrajkot', ' One new initiative, where donors are sent automatic text messages telling them when their blood has actually been used, has caught the public eye.', '2018-08-07 03:55:14', 1),
(9, 'vala gaurav maganbhai', '9624857559', 'valagaurav111@gmail.com', 'Male', 19, 'A+', '"khodiyar krupa"\r\n''150 ft. ring road\r\nrajkot', ' One new initiative, where donors are sent automatic text messages telling them when their blood has actually been used, has caught the public eye.', '2018-08-07 03:57:13', 1),
(10, 'prashant kanjariya', '8154864173', 'prashantkanjariya47@gmail.com', 'Male', 19, 'b+', 'akshar nagar -2\r\ngandhigram \r\nrajkot-360007', '“We are constantly trying to develop ways to express [donors''] importance,” Karolina Blom Wiberg, a communications manager at the Stockholm blood service told The Independent. ', '2018-08-07 04:00:39', 1),
(11, 'tejash vadoliya', '88666209365', 'tp123@gmail.com', 'Male', 20, 'b+', 'kastbhanjan main road\r\ngandhigram \r\nrajkot 360007', ' People who donate initially receive a ''thank you'' text when they give blood, but they get another message when their blood makes it into somebody else’s veins.', '2018-08-07 04:02:12', 1),
(12, 'tejash vadoliya', '88666209365', 'tp123@gmail.com', 'Male', 20, 'b-', 'kastbhanjan main road\r\ngandhigram \r\nrajkot 360007', ' People who donate initially receive a ''thank you'' text when they give blood, but they get another message when their blood makes it into somebody else’s veins.', '2018-08-07 04:03:25', 1),
(13, 'parth jataniya', '88777225154', 'parth123@gmail.com', 'Male', 21, 'o+', 'ramnath society main road\r\njam khambhadiya', '“We want to give them feed back on their effort, and we find this is a good way to do that.” ', '2018-08-07 04:04:42', 1),
(14, 'rohan peshavariya', '6321457899', 'rjp123@gmail.com', 'Male', 20, 'AB-', 'gokuldham main road\r\nrajkot 360003', 'The service says the messages give donors more positive feedback about how they’ve helped their fellow citizens – which encourages them to donate again. ', '2018-08-07 04:06:29', 1),
(15, 'rani patel', '9876543210', 'rpatel123@gmail.com', 'Female', 22, 'O-', 'university road\r\nrajkot 360007', 'he new policy has also been a hit on social media and has got people talking about blood donation amongst their friends. ', '2018-08-07 04:08:21', 1),
(16, 'kinjal prajapati', '8899123456', 'kp123@gmail.com', 'Male', 25, 'AB-', 'kalwad road\r\nnr. shoping center\r\nrajkot', 'blood donation rates in decline all over the developed world, Sweden’s blood service is enlisting new technology to help push back against shortages. ', '2018-08-07 04:09:54', 1),
(17, 'kriswa kachhela', '98987515247', 'kkachhela47@gmail.com', 'Female', 23, 'O+', 'mavdi main road\r\nrajkot', 'they get another message when their blood makes it into somebody else’s veins. ', '2018-08-07 04:12:16', 1),
(18, 'raj timabadiya', '6565478291', 'raj123@gmail.com', 'Male', 28, 'A-', 'raiya road\r\nrajkot', 'One new initiative, where donors are sent automatic text messages telling them when their blood has actually been used, has caught the public eye. ', '2018-08-07 04:23:53', 1);

-- --------------------------------------------------------

--
-- Table structure for table `tblbloodgroup`
--

CREATE TABLE IF NOT EXISTS `tblbloodgroup` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `BloodGroup` varchar(20) DEFAULT NULL,
  `PostingDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=9 ;

--
-- Dumping data for table `tblbloodgroup`
--

INSERT INTO `tblbloodgroup` (`id`, `BloodGroup`, `PostingDate`) VALUES
(1, 'A-', '2017-06-30 20:33:50'),
(2, 'AB-', '2017-06-30 20:34:00'),
(3, 'O-', '2017-06-30 20:34:05'),
(4, 'A-', '2017-06-30 20:34:10'),
(5, 'A+', '2017-06-30 20:34:13'),
(6, 'AB+', '2017-06-30 20:34:18'),
(7, 'b+', '2018-08-07 03:57:05'),
(8, 'o+', '2018-08-07 04:03:17');

-- --------------------------------------------------------

--
-- Table structure for table `tblcontactusinfo`
--

CREATE TABLE IF NOT EXISTS `tblcontactusinfo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `Address` tinytext,
  `EmailId` varchar(255) DEFAULT NULL,
  `ContactNo` char(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Dumping data for table `tblcontactusinfo`
--

INSERT INTO `tblcontactusinfo` (`id`, `Address`, `EmailId`, `ContactNo`) VALUES
(1, 'Test Demo test demo																									', 'test@test.com', '8585233222');

-- --------------------------------------------------------

--
-- Table structure for table `tblcontactusquery`
--

CREATE TABLE IF NOT EXISTS `tblcontactusquery` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(100) DEFAULT NULL,
  `EmailId` varchar(120) DEFAULT NULL,
  `ContactNumber` char(11) DEFAULT NULL,
  `Message` longtext,
  `PostingDate` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `status` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=5 ;

--
-- Dumping data for table `tblcontactusquery`
--

INSERT INTO `tblcontactusquery` (`id`, `name`, `EmailId`, `ContactNumber`, `Message`, `PostingDate`, `status`) VALUES
(1, 'Anuj Kumar', 'webhostingamigo@gmail.com', '2147483647', 'Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry''s standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum', '2017-06-18 10:03:07', 1),
(2, 'caasda', 'webhostingamigo@gmail.com', '42342342', 'drftghjk', '2017-06-30 21:17:09', NULL),
(3, 'caasda', 'webhostingamigo@gmail.com', '42342342', 'drftghjk', '2017-06-30 21:21:30', NULL),
(4, 'te', 'sdfsdf@gmail.com', '75787875545', 'sfsdf sdg hs h sh', '2017-07-01 07:19:36', NULL);

-- --------------------------------------------------------

--
-- Table structure for table `tblpages`
--

CREATE TABLE IF NOT EXISTS `tblpages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `PageName` varchar(255) DEFAULT NULL,
  `type` varchar(255) NOT NULL DEFAULT '',
  `detail` longtext NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=latin1 AUTO_INCREMENT=22 ;

--
-- Dumping data for table `tblpages`
--

INSERT INTO `tblpages` (`id`, `PageName`, `type`, `detail`) VALUES
(2, 'Why Become Donor', 'donor', '<span style="color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif; font-size: 14px; text-align: justify;">At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat</span>'),
(3, 'About Us ', 'aboutus', '<span style="color: rgb(0, 0, 0); font-family: &quot;Open Sans&quot;, Arial, sans-serif; text-align: justify;">At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat</span>');

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
